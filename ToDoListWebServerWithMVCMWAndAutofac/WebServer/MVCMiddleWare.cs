using Autofac;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ToDoListWebServerWithMVCMWAndAutofac.Server;
using ToDoListWebServerWithMVCMWAndAutofac.WebServer;
using ToDoListWebServerWithMVCMWAndAutofac.WebServer.Attributes;

namespace ToDoListWebServerWithMVCMWAndAutofac.WebServer
{
    public class MVCMiddleWare : IMiddleWare
    {
        private DelegateMiddleWare next;

        public MVCMiddleWare(DelegateMiddleWare next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            Console.WriteLine("enter MVCMiddleWare " + context.Request.Url.AbsoluteUri);

            var responce = context.Response;
            var writer = new StreamWriter(responce.OutputStream);

            try
            {
                string resp = Execute(context.Request, data);

                if (resp != null)
                {
                    responce.StatusCode = 200;
                    responce.ContentType = "text/html";
                    writer.Write(resp);
                }
                else
                {
                    await next.Invoke(context, data);
                }
            }
            catch (Exception ex)
            {
                responce.StatusCode = 500;
                responce.ContentType = "text/plain";
                writer.Write(ex.Message);
            }
            finally
            {
                writer.Close();
            }


            //var result = Execute(context.Request, data);

            //if (result != null)
            //{
            //    responce.ContentType = "text/html";
            //    responce.StatusCode = 200;
            //    await writer.WriteAsync(result);
            //    writer.Close();

            //    Console.WriteLine("exit MVCMiddleWare");
            //}
            //else
            //    await next.Invoke(context, data);
        }

        private string Execute(HttpListenerRequest request, Dictionary<string, object> data)
        {
            var urlParts = request.Url.PathAndQuery.Split(new[] { '/', '\\', '?' }, StringSplitOptions.RemoveEmptyEntries);

            if (urlParts.Length < 2)
                return null;

            var controller = urlParts[0];
            var action = urlParts[1];

            //var ctrlName = $"ToDoListWebServerWithMVCMiddleWare.Controllers.{controller}Controller";
            //var asm = Assembly.GetExecutingAssembly();
            //var controllerType = asm.GetType(ctrlName, false, true); // last Capitalize

            Assembly curAssembly = Assembly.GetExecutingAssembly();

            Type controllerType = curAssembly.GetType($"ToDoListWebServerWithMVCMWAndAutofac.Controllers.{controller}Controller", false, true);

            if (controllerType is null)
                return null;

            var actionMethod = controllerType.GetMethod(action, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (actionMethod is null)
                return null;

            var attr = actionMethod.GetCustomAttribute<HttpMethodAttribute>();

            if (attr != null)
            {
                if (String.Compare(attr.Method, request.HttpMethod, true) != 0)
                {
                    Console.WriteLine($"{attr.Method} - {request.HttpMethod}");
                    return null;
                }

            }

            var attrAuth = actionMethod.GetCustomAttribute<AuthorizeAttribute>();

            if (attrAuth != null)
            {
                if ((bool)data["isAuth"] == false && controller == "user")
                {
                    //return "HTTP ERROR 401: Not authorizade";
                    return "<script>window.location = 'http://127.0.0.1:5600/user/login'</script>";
                }

                if (attrAuth.Roles != null)
                {
                    var roles = attrAuth.Roles.Split(',');
                    if (!roles.Contains(data["Role"]))
                    {
                        return "HTTP ERROR 401: Accec Denied!";
                    }
                }
            }

            var controllerInstance = Activator.CreateInstance(controllerType); // new PhonesController

            var args = new List<object>();
            NameValueCollection queryParams = null;

            if (request.HttpMethod == "GET") // url HttpMethod.Get.Method
            {
                if (urlParts.Length == 2 && actionMethod.GetParameters().Length != 0)
                    return null;

                if (urlParts.Length > 2)
                    queryParams = HttpUtility.ParseQueryString(urlParts[2]);
            }
            else if (request.HttpMethod == "POST") // form HttpMethod.Post.Method
            {
                //using (StreamReader sr = new StreamReader(request.InputStream))
                //{
                //    var dataForm = sr.ReadToEnd();
                //    queryParams = HttpUtility.ParseQueryString(dataForm);
                //}

                string body;

                using (StreamReader reader = new StreamReader(request.InputStream))
                {
                    body = reader.ReadToEnd();
                }
                queryParams = HttpUtility.ParseQueryString(body);
            }
            else
                return null;

            var parameters = actionMethod.GetParameters();

            foreach (var param in parameters)
            {
                //var item = queryParams[param.Name];
                //var arg = Convert.ChangeType(item, param.ParameterType);
                //args.Add(arg);

                args.Add(Convert.ChangeType(queryParams[param.Name], param.ParameterType));
            }

            if (args.Count != actionMethod.GetParameters().Length)
                return null;

            //var r = (string)actionMethod.Invoke(controllerInstance, args.ToArray());
            
            var _this = CustomWebServer.IOC.Resolve(controllerType);
            var methodArgs = args.ToArray();
            var res = actionMethod.Invoke(_this, methodArgs);

            return res as string;
        }
    }
}