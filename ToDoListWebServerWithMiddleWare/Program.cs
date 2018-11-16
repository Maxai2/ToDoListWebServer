using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebServerWithMiddleWare.Server;

namespace ToDoListWebServerWithMiddleWare
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomWebServer webServer = new CustomWebServer("127.0.0.1", 5600);
            //webServer.Configure

            webServer.Start();

            Console.WriteLine("Web Server Start...");

            try
            {
                webServer.Listen();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Web server stopped...");
                Console.WriteLine(ex.Message);
                webServer.Stop();
            }
        }
    }
}
