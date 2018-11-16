using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using ToDoListWebServerWithMiddleWare.Services;

namespace ToDoListWebServerWithMiddleWare.Server
{
    class CustomWebServer
    {
        private readonly string ip;
        private readonly int port;

        private HttpListener listener;
        ToDoService doService;

        public CustomWebServer(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            listener = new HttpListener();
            doService = new ToDoService();

            listener.Prefixes.Add($"http://{ip}:{port}/");
        }

        public void Start()
        {
            if (listener != null)
                listener.Start();
        }

        public void Stop()
        {
            if (listener != null)
                listener.Stop();
        }

        public void Listen()
        {
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                string path = context.Request.Url.AbsolutePath;
                string method = context.Request.HttpMethod;

                if (method == HttpMethod.Get.Method && path == "/toDoList")
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append("<ol>");

                    int count = 0;

                    foreach (var elem in doService.GetToDoList())
                    {
                        strBuilder.Append($"<li>{elem.itemName}<form method='POST' action='http://127.0.0.1:5600/checkToDo'> <input type='checkbox' name='check' {elem.itemState}> <input type='hidden' name='id' value={count}> <input type='submit'> </form> </li>");
                        count++;
                    }

                    strBuilder.Append("</ol>");
                    strBuilder.Append("<form method='POST' action='http://127.0.0.1:5600/addToDo'> <label>To do: </label> <input type='text' name='toDoName' required> <input type='submit'> </form>");

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/html";

                    using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
                    {
                        sw.Write(strBuilder.ToString());
                    }
                }
                else
                if (method == HttpMethod.Post.Method && path == "/checkToDo")
                {
                    NameValueCollection res = getResult(context);
                    int index = int.Parse(res["id"]);
                    string che = res["check"];
                    doService.changeToDoState(index, che);
                    context.Response.Redirect($"http://{ip}:{port}/toDoList");
                }
                else
                if (method == HttpMethod.Post.Method && path == "/addToDo")
                {
                    NameValueCollection res = getResult(context);
                    string toDo = res["toDoName"];
                    doService.addToDoItem(toDo);
                    context.Response.Redirect($"http://{ip}:{port}/toDoList");
                }
                else
                {
                    context.Response.StatusCode = 404;
                }

                context.Response.Close();

            }
        }

        private NameValueCollection getResult(HttpListenerContext context)
        {
            string data;

            using (StreamReader sr = new StreamReader(context.Request.InputStream))
            {
                data = sr.ReadToEnd();
            }

            return HttpUtility.ParseQueryString(data);
        }
    }
}
