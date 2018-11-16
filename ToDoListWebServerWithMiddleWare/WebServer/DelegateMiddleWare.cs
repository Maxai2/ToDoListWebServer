using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMiddleWare.WebServer
{
    public delegate Task DelegateMiddleWare(HttpListenerContext context, Dictionary<string, object> data);
}
