namespace ToDoListWebServerWithMiddleWare.WebServer
{
    public interface IConfigurator
    {
        void ConfiguteMiddleWare(MiddleWareBuilder builder);
    }

    public class Configurator : IConfigurator
    {
        public void ConfiguteMiddleWare(MiddleWareBuilder builder)
        {
            builder.Use<AuthorizationMiddleWare>().Use<DynamicMiddleWare>();
        }
    }
}
