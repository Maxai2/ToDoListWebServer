using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using ToDoListWebServerWithMiddleWare.Services;
using ToDoListWebServerWithMiddleWare.WebServer;

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

        private DelegateMiddleWare firstMiddleWare;

        public CustomWebServer Configure<T>() where T : IConfigurator, new()
        {
            IConfigurator configurator = new T();
            MiddleWareBuilder builder = new MiddleWareBuilder();

            configurator.ConfigureMiddleWare(builder);
            firstMiddleWare = builder.Build();
            return this;
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

        public async void Listen()
        {
            while (true)
            {
                HttpListenerContext context = listener.GetContext();

                await firstMiddleWare.Invoke(context, new Dictionary<string, object>());

                context.Response.Close();
            }
        }
    }
}
