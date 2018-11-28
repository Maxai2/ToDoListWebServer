using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMVCMWAndAutofac.WebServer
{
    public delegate Task DelegateMiddleWare(HttpListenerContext context, Dictionary<string, object> data);
}
