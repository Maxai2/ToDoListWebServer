using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using ToDoListWebServerWithMVCMWAndAutofac.Services;
using ToDoListWebServerWithMVCMWAndAutofac.WebServer;

namespace ToDoListWebServerWithMVCMWAndAutofac.Server
{
    class CustomWebServer
    {
        private string ip;
        private int port;

        private HttpListener listener;

        public CustomWebServer(string ip, int port)
        {
            this.ip = ip;
            this.port = port;

            listener = new HttpListener();

            listener.Prefixes.Add($"http://{ip}:{port}/");
        }

        private DelegateMiddleWare firstMiddleWare;

        public static IContainer IOC { get; private set; }

        public CustomWebServer Configure<T>() where T : IConfigurator, new()
        {
            IConfigurator configurator = new T();

            var builder = new MiddleWareBuilder();
            configurator.ConfigureMiddleWare(builder);
            firstMiddleWare = builder.Build();

            var depBuilder = new ContainerBuilder();
            configurator.ConfigureDependencies(depBuilder);
            IOC = depBuilder.Build();

            return this;
        }

        public void Run()
        {
            listener.Start();

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                Task.Run(() => { Process(context); });
            }
        }

        private void Process(HttpListenerContext context)
        {
            try
            {
                firstMiddleWare.Invoke(context, new Dictionary<string, object>());
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";

                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                {
                    writer.Write($"Error on CustomWebServer: {ex.Message}");
                }
            }
        }

        //public void Start()
        //{
        //    if (listener != null)
        //        listener.Start();
        //}

        //public void Stop()
        //{
        //    if (listener != null)
        //        listener.Stop();
        //}

        //public async void Listen()
        //{
        //    while (true)
        //    {
        //        HttpListenerContext context = listener.GetContext();

        //        await firstMiddleWare.Invoke(context, new Dictionary<string, object>());

        //        context.Response.Close();
        //    }
        //}
    }
}
