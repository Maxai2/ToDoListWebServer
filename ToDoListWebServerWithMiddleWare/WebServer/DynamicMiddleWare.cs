using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ToDoListWebServerWithMiddleWare.Services;

namespace ToDoListWebServerWithMiddleWare.WebServer
{
    class DynamicMiddleWare : IMiddleWare
    {
        private DelegateMiddleWare next;

        public DynamicMiddleWare(DelegateMiddleWare next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            Console.WriteLine("enter dynamicMW " + context.Request.Url.AbsoluteUri);

            string path = context.Request.Url.AbsolutePath;
            string method = context.Request.HttpMethod;

            if (method == HttpMethod.Get.Method && path == "/login")
            {
                StringBuilder strBuilder = new StringBuilder();

                strBuilder.Append("<form method='POST' action='http://127.0.0.1:5600/login' style='margin-left: 0 auto;'>" +
                                        "<br>" +
                                        "<label style = 'font-weight: bold; font-size: 20px;' > Login </ label >" +
                                        "<br>" +
                                        "<br>" +
                                        "<input type = 'text' name = 'login' placeholder = 'Login' required >" +
                                        "<br>" +
                                        "<br>" +
                                        "<input type = 'text' name = 'password' placeholder = 'Password' required >" +
                                        "<br>" +
                                        "<br>" +
                                        "<input type = 'submit' value = 'Enter' ></ input >" +
                                   "</form> ");

                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html";

                using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
                {
                    sw.Write(strBuilder.ToString());
                }
            }
            else
            if (method == HttpMethod.Post.Method && path == "/login")
            {
                string query;

                using (StreamReader sr = new StreamReader(context.Request.InputStream))
                {
                    query = sr.ReadToEnd();
                }

                NameValueCollection res = HttpUtility.ParseQueryString(query);

                string login = res["login"];
                string pass = res["password"];

                var userIndex = UserService.getUsers().FindIndex(u => (u.Name == login) && (u.Password == pass));

                if (userIndex != -1)
                {
                    UserService.UserId = userIndex;

                    byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                    byte[] key = Guid.NewGuid().ToByteArray();

                    string token = Convert.ToBase64String(time.Concat(key).ToArray());
                    UserService.UserToken = token;

                    context.Response.Redirect($"http://127.0.0.1:5600/home?token={token}");
                }
                else
                    context.Response.Redirect($"http://127.0.0.1:5600/login");
            }
            else
            if (method == HttpMethod.Get.Method && path == "/home")
            {
                if ((bool)data["isAuth"] == false)
                {
                    context.Response.Redirect($"http://127.0.0.1:5600/login");
                }

                StringBuilder strBuilder = new StringBuilder();

                string query;

                using (StreamReader sr = new StreamReader(context.Request.InputStream))
                {
                    query = sr.ReadToEnd();
                }

                NameValueCollection res = HttpUtility.ParseQueryString(query);

                string token = res["token"];

                strBuilder.Append("<div style='margin: 0 auto;'>" +
                                      "<br>" +
                                      "<label style = 'font-weight: bold; font-size: 20px;'> Home </label>" +
                                      "<br>" +
                                      "<br>" +
                                      $"<form method = 'GET' action = 'http://127.0.0.1:5600/toDoList?token={token}'>" +
                                        "<input type = 'submit' value = 'List ToDo' style = 'height: 30px; width: 100px; background-color: transparent;'>" +
                                      "</form>" +
                                      "<form method = 'GET' action = 'http://127.0.0.1:5600/login'>" +
                                        "<input type = 'submit' value = 'Exit' style = 'height: 30px; width: 100px; background-color: transparent;'>" +
                                      "</form>" +
                                "</div>");

                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html";

                using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
                {
                    sw.Write(strBuilder.ToString());
                }
            }
            else
            if (method == HttpMethod.Get.Method && path == "/toDoList")
            {
                if ((bool)data["isAuth"] == false)
                {
                    context.Response.Redirect($"http://127.0.0.1:5600/login");
                }

                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("<ol>");

                //int count = 0;

                //var userList = new ToDoService();

                //foreach (var elem in userList.GetUserToDoList())
                //{
                //    strBuilder.Append($"<li>{elem.ItemName}<form method='POST' action='http://127.0.0.1:5600/toDoList?method=changeState'> <input type='checkbox' name='check' {elem.ItemState}> <input type='hidden' name='id' value={count}> <input type='submit'> </form> </li>");
                //    count++;
                //}

                //strBuilder.Append("</ol>");
                strBuilder.Append("<form method='POST' action='http://127.0.0.1:5600/toDoList?method=addToDo'> <label>To do: </label> <input type='text' name='toDoName' required> <input type='submit' value='Add'> </form>");
                strBuilder.Append("<form method='GET' action='http://127.0.0.1:5600/login'> <input type='submit' value='Log Out'> </form>");

                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html";

                using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
                {
                    sw.Write(strBuilder.ToString());
                }
            }
            else
            if (method == HttpMethod.Post.Method && path == "/toDoList?method=changeState")
            {
                StringBuilder strBuilder = new StringBuilder();

                string query;

                using (StreamReader sr = new StreamReader(context.Request.InputStream))
                {
                    query = sr.ReadToEnd();
                }

                NameValueCollection res = HttpUtility.ParseQueryString(query);

                int index = int.Parse(res["id"]);
                string che = res["check"];

                ToDoService toDoService = new ToDoService();

                toDoService.changeToDoState(index, che);
                context.Response.Redirect($"http://127.0.0.1:5600/toDoList");
            }
            else
            if (method == HttpMethod.Post.Method && path == "/toDoList?method=addToDo")
            {
                StringBuilder strBuilder = new StringBuilder();

                string query;

                using (StreamReader sr = new StreamReader(context.Request.InputStream))
                {
                    query = sr.ReadToEnd();
                }

                NameValueCollection res = HttpUtility.ParseQueryString(query);

                string toDo = res["toDoName"];

                ToDoService toDoService = new ToDoService();
                toDoService.addToDoItem(toDo);
                context.Response.Redirect($"http://127.0.0.1:5600/toDoList");
            }
            else
            {
                context.Response.StatusCode = 404;
            }

            if (next != null)
            {
                await next.Invoke(context, data);
            }

            Console.WriteLine("Exit dynamicMW");
        }
    }
}
