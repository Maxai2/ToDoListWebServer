using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebServerWithMVCMWAndAutofac.Services;

namespace ToDoListWebServerWithMVCMWAndAutofac.Controllers
{
    class HomeController
    {
        private Dictionary<string, object> data;

        public HomeController(Dictionary<string, object> data)
        {
            this.data = data;
        }

        public string enter()
        {
            if ((bool)data["isAuth"] == false)
                return "<script>window.location = 'http://127.0.0.1:5600/user/login'</script>";
            else
                return "<div style='margin: 0 auto;'>" +
                                          "<br>" +
                                          "<label style = 'font-weight: bold; font-size: 20px;'> Home </label>" +
                                          "<br>" +
                                          "<br>" +
                                          $"<a href='http://127.0.0.1:5600/toDo/showList?token={UserService.UserToken}' style = 'height: 30px; width: 100px; background-color: greenyellow'>List ToDo</a>" +
                                          "<br>" +
                                          "<br>" +
                                          "<a href='http://127.0.0.1:5600/user/login' style = 'height: 30px; width: 100px;'>Exit</a>" +
                                    "</div>";
        }
    }
}
