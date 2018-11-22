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
            builder.Use<DynamicMiddleWare>().Use<AuthorizationMiddleWare>(); 
        }
    }
}
