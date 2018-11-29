using Autofac;
using ToDoListWebServerWithMVCMWAndAutofac.Controllers;
using ToDoListWebServerWithMVCMWAndAutofac.Services;
using ToDoListWebServerWithMVCMWAndAutofac.WebServer;

namespace ToDoListWebServerWithMVCMWAndAutofac.WebServer
{
    public interface IConfigurator
    {
        void ConfigureMiddleWare(MiddleWareBuilder builder);
        void ConfigureDependencies(ContainerBuilder builder);
    }

    public class Configurator : IConfigurator
    {
        public void ConfigureDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<UserController>();
            builder.RegisterType<HomeController>();
            builder.RegisterType<ToDoController>();

            builder.RegisterType<ToDoService>().As<IToDoService>();
            builder.RegisterType<UserService>().As<IUserservice>();
        }

        public void ConfigureMiddleWare(MiddleWareBuilder builder)
        {
            builder.Use<StaticFilesMiddleware>().Use<AuthorizationMiddleWare>().Use<MVCMiddleWare>(); 
        }
    }
}
