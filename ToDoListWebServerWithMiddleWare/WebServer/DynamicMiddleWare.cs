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

                strBuilder.Append("<form method='POST' action='http//127.0.0.1:5600/login' style='margin-left: 0 auto;'>" +
                                        "<br>" +
                                        "<label style = 'font-weight: bold; font-size: 20px;' > Login </ label >" +
                                        "<br>" +
                                        "<br>" +
                                        "<input type = 'text' name = 'login' placeholder = 'Login' required >" +
                                        "<br>" +
                                        "<br>" +
                                        "<input type = 'password' name = 'password' placeholder = 'Password' required >" +
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
                //NameValueCollection res = getResult(context);

                string login = context.Request.QueryString["login"];
                string pass = context.Request.QueryString["password"];

                UserService userService = new UserService();

                if (userService.getUsers().Find(u => (u.Name == login) && (u.Password == pass)) != null)
                {

                }

                //doService.changeToDoState(index, che);

                //context.Response.Redirect($"http://{ip}:{port}/toDoList");
            }
            else
            if (method == HttpMethod.Get.Method && path == "/home")
            {
                StringBuilder strBuilder = new StringBuilder();

                strBuilder.Append("< div style='margin: 0 auto;' >" +
                                      "< br >" +
                                      "< label style = 'font-weight: bold; font-size: 20px;' > Home </ label >" +
                                      "< br >" +
                                      "< br >" +
                                      "< form method = 'GET' action = 'http://127.0.0.1/toDoList?token=qwerty' >" +
                                        "< input type = 'submit' value = 'List ToDo' style = 'height: 30px; width: 100px; background-color: transparent;' >" +
                                      "</ form >" +
                                      "< br >" +
                                      "< form method = 'GET' action = 'http://127.0.0.1/login' >" +
                                        "< input type = 'submit' value = 'Exit' style = 'height: 30px; width: 100px; background-color: transparent;' >" +
                                      "</ form >" +
                                "</ div > ");

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
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("<ol>");

                int count = 0;

                ToDoService toDoService = new ToDoService();

                foreach (var elem in toDoService.GetUserToDoList())
                {
                    strBuilder.Append($"<li>{elem.ItemName}<form method='POST' action='http://127.0.0.1:5600/toDoList?method=changeState'> <input type='checkbox' name='check' {elem.ItemState}> <input type='hidden' name='id' value={count}> <input type='submit'> </form> </li>");
                    count++;
                }

                strBuilder.Append("</ol>");
                strBuilder.Append("<form method='POST' action='http://127.0.0.1:5600/toDoList?method=addToDo'> <label>To do: </label> <input type='text' name='toDoName' required> <input type='submit'> </form>");

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
                int index = int.Parse(context.Request.QueryString["id"]);
                string che = context.Request.QueryString["check"];

                ToDoService toDoService = new ToDoService();

                toDoService.changeToDoState(index, che);
                context.Response.Redirect($"http://127.0.0.1:5600/toDoList");
            }
            else
            if (method == HttpMethod.Post.Method && path == "/toDoList?method=addToDo")
            {
                string toDo = context.Request.QueryString["toDoName"];

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
