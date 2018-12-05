using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebServerWithMVCMWAndAutofac.Services;
using ToDoListWebServerWithMVCMWAndAutofac.WebServer.Attributes;

namespace ToDoListWebServerWithMVCMWAndAutofac.Controllers
{
    class HomeController
    {
        [Authorize("User,Admin")]
        [HttpMethod("GET")]
        public string enter()
        {
            if (UserService.ErrorMes == "")
            {
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
            else
            {
                return "<div>" +
                    $"<span style='color: red;'>{UserService.ErrorMes}</span> " +
                    "<br/>" +
                    "<a href='http://127.0.0.1:5600/user/login'>Back to Login</a>" +
                    "</div>";
            }
        }
    }
}