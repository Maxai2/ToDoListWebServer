using ToDoListWebServerWithMVCMiddleWare.WebServer;

namespace ToDoListWebServerWithMiddleWare.WebServer
{
    public interface IConfigurator
    {
        void ConfigureMiddleWare(MiddleWareBuilder builder);
    }

    public class Configurator : IConfigurator
    {
        public void ConfigureMiddleWare(MiddleWareBuilder builder)
        {
            builder.Use<AuthorizationMiddleWare>().Use<MVCMiddleWare>(); 
        }
    }
}
