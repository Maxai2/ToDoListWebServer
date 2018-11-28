using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebServerWithMVCMWAndAutofac.Server;
using ToDoListWebServerWithMVCMWAndAutofac.WebServer;

namespace ToDoListWebServerWithMVCMWAndAutofac
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Server is running...");
                CustomWebServer webServer = new CustomWebServer("127.0.0.1", 5600);
                webServer.Configure<Configurator>().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server stoped");
                Console.WriteLine(ex.Message);
            }
        }
    }
}