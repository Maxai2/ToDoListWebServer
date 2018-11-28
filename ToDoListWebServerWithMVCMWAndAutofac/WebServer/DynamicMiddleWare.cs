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
using ToDoListWebServerWithMVCMWAndAutofac.Services;

namespace ToDoListWebServerWithMVCMWAndAutofac.WebServer
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
            string pathQuery = context.Request.Url.Query;
            string method = context.Request.HttpMethod;

            if (method == HttpMethod.Get.Method && path == "/login")
            {
                StringBuilder strBuilder = new StringBuilder();

                strBuilder.Append("<form method='POST' action='/login' style='margin-left: 0 auto;'>" +
                                        "<br>" +
                                        "<label style = 'font-weight: bold; font-size: 20px;'>Login</label>" +
                                        "<br>" +
                                        "<br>" +
                                        "<input type = 'text' name = 'login' placeholder = 'Login' required/>" +
                                        "<br>" +
                                        "<br>" +
                                        "<input type = 'text' name = 'password' placeholder = 'Password' required/>" +
                                        "<br>" +
                                        "<br>" +
                                        "<input type = 'submit' value = 'Enter'/>" +
                                   "</form>");

                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/html";

                using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
                {
                    sw.Write(strBuilder.ToString());
                }
            }
            else if (method == HttpMethod.Post.Method && path == "/login")
            {
                string query;

                using (StreamReader sr = new StreamReader(context.Request.InputStream))
                {
                    query = sr.ReadToEnd();
                }

                NameValueCollection res = HttpUtility.ParseQueryString(query);

                string login = res["login"];
                string pass = res["password"];

                var userS = new UserService();

                var userIndex = userS.getUsers().FindIndex(u => (u.Name == login) && (u.Password == pass));

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
                    context.Response.Redirect("http://127.0.0.1:5600/login");
            }
            else if (method == HttpMethod.Get.Method && path == "/home" && pathQuery == $"?token={UserService.UserToken}")
            {
                if ((bool)data["isAuth"] == false)
                {
                    context.Response.Redirect("http://127.0.0.1:5600/login");
                    //goto escapeIf;
                }
                else
                {
                    StringBuilder strBuilder = new StringBuilder();

                    strBuilder.Append("<div style='margin: 0 auto;'>" +
                                          "<br>"+
                                          "<label style = 'font-weight: bold; font-size: 20px;'> Home </label>" +
                                          "<br>"+
                                          "<br>"+
                                          $"<a href='http://127.0.0.1:5600/toDoList?token={UserService.UserToken}' style = 'height: 30px; width: 100px; background-color: greenyellow'>List ToDo</a>" +
                                          "<br>"+
                                          "<br>"+
                                          "<a href='http://127.0.0.1:5600/login' style = 'height: 30px; width: 100px;'>Exit</a>" +
                                    "</div>");

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/html";

                    using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
                    {
                        sw.Write(strBuilder.ToString());
                    }
                }
            }
            else if (method == HttpMethod.Get.Method && path == "/toDoList" && pathQuery == $"?token={UserService.UserToken}")
            {
                if ((bool)data["isAuth"] == false)
                {
                    context.Response.Redirect("http://127.0.0.1:5600/login");
                    //goto escapeIf;
                }
                else
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("<ol>");

                    int count = 0;

                    var userList = new ToDoService();

                    foreach (var elem in userList.GetUserToDoList())
                    {
                        strBuilder.Append($"<li>{elem.ItemName}<form method='POST' action='http://127.0.0.1:5600/toDoList?method=changeState&token={UserService.UserToken}'> <input type='checkbox' name='check' {elem.ItemState}> <input type='hidden' name='id' value={count}> <input type='submit'> </form> </li>");
                        count++;
                    }

                    strBuilder.Append("</ol>");
                    strBuilder.Append($"<form method='POST' action='http://127.0.0.1:5600/toDoList?method=addToDo&token={UserService.UserToken}'> <label>To do: </label> <input type='text' name='toDoName' required> <input type='submit' value='Add'> </form>");
                    strBuilder.Append("<form method='GET' action='http://127.0.0.1:5600/login'> <input type='submit' value='Log Out'> </form>");

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/html";

                    using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
                    {
                        sw.Write(strBuilder.ToString());
                    }
                }
            }
            else if (method == HttpMethod.Post.Method && path == "/toDoList" && pathQuery == $"?method=changeState&token={UserService.UserToken}")
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

                var toDoS = new ToDoService();

                toDoS.changeToDoState(index, che);
                context.Response.Redirect($"http://127.0.0.1:5600/toDoList?token={UserService.UserToken}");
            }
            else if (method == HttpMethod.Post.Method && path == "/toDoList" && pathQuery == $"?method=addToDo&token={UserService.UserToken}")
            {
                StringBuilder strBuilder = new StringBuilder();

                string query;

                using (StreamReader sr = new StreamReader(context.Request.InputStream))
                {
                    query = sr.ReadToEnd();
                }

                NameValueCollection res = HttpUtility.ParseQueryString(query);

                string toDo = res["toDoName"];

                var toDoS = new ToDoService();

                toDoS.addToDoItem(toDo);
                context.Response.Redirect($"http://127.0.0.1:5600/toDoList?token={UserService.UserToken}");
            }
            else
            {
                context.Response.StatusCode = 404;
            }

            //escapeIf:

            if (next != null)
            {
                await next.Invoke(context, data);
            }

            Console.WriteLine("Exit dynamicMW");
        }
    }
}
