using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMiddleWare.WebServer
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

            if (next != null)
            {
                await next.Invoke(context, data);
            }

            Console.WriteLine("exit authorizationMW");
        }
    }
}
