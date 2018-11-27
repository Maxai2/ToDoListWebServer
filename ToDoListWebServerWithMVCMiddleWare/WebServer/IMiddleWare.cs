using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMiddleWare.WebServer
{
    public interface IMiddleWare
    {
        Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data);
    }
}