using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ToDoListWebServerWithMVCMWAndAutofac.Services;

namespace ToDoListWebServerWithMVCMWAndAutofac.WebServer
{
    class AuthorizationMiddleWare : IMiddleWare
    {
        private DelegateMiddleWare next;

        public AuthorizationMiddleWare(DelegateMiddleWare next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            Console.WriteLine("enter authorizationMW " + context.Request.Url.AbsoluteUri);

            var cookie = context.Request.Cookies;

            string token = "";
            string role = "";

            if (cookie != null)
            {
                token = cookie["token"]?.Value;
                role = cookie["role"]?.Value;
            }

            //var token = context.Request.Cookies["token"];

            //data.Add("isAuth", token != null ? true : false);

            //if (token != null)

            if (token != "" && token != null)
                data.Add("isAuth", true);
            else
                data.Add("isAuth", false);

            //if (next != null)
            //{
            //    await next.Invoke(context, data);
            //}

            //var role = context.Request.QueryString["role"];

            //data.Add("Role", role?.Value ?? "Guest");

            if (role != "" && role != null)
                data.Add("Role", role);
            else
                data.Add("Role", "Guest");

            await next.Invoke(context, data);

            Console.WriteLine("exit authorizationMW");
        }
    }
}