using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMVCMWAndAutofac.WebServer
{
    public interface IMiddleWare
    {
        Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data);
    }
}