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

            string token = context.Request.QueryString["token"];

            if (token != "" && token != null && token == UserService.UserToken)
                data.Add("isAuth", true);
            else
                data.Add("isAuth", false);

            if (next != null)
            {
                await next.Invoke(context, data);
            }

            Console.WriteLine("exit authorizationMW");
        }
    }
}
