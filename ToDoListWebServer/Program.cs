using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebServer.Server;

namespace ToDoListWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ToDoWebServer webServer = new ToDoWebServer("127.0.0.1", 5600);

            webServer.Start();

            Console.WriteLine("Web server start...");

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
