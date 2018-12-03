using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebServerWithMVCMWAndAutofac.Services;
using ToDoListWebServerWithMVCMWAndAutofac.WebServer.Attributes;

namespace ToDoListWebServerWithMVCMWAndAutofac.Controllers
{
    class UserController
    {
        [HttpMethod("GET")]
        public string login()
        {
            return "<form method='POST' action='http://127.0.0.1:5600/user/checkLogin' style='margin-left: 0 auto;'>" +
                                        "<br>" +
                                        "<label style = 'font-weight: bold; font-size: 20px;'>Login</label>" +
                                        "<br>" +
                                        "<br>" +
                                        "<input type = 'text' name = 'login' placeholder = 'Login' required/>" +
                                        "<br>" +
                                        "<br>" +
                                        "<input type = 'text' name = 'password' placeholder = 'Password' required/>" +
                                        "<br>" +
                                        "<br>" +
                                        "<input type = 'submit' value = 'Enter'/>" +
                   "</form>" +
                   $"<span style='color: red;'>{UserService.ErrorMes}</span>";
        }
        
        [HttpMethod("POST")]
        public string checkLogin(string login, string password)
        {
            //var userS = new UserService();

            //var userIndex = userS.getUsers().FindIndex(u => (u.Name == login) && (u.Password == password));

            //if (userIndex != -1)
            //{
            //    UserService.UserId = userIndex;

            //    byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            //    byte[] key = Guid.NewGuid().ToByteArray();

            //    string token = Convert.ToBase64String(time.Concat(key).ToArray());
            //    UserService.UserToken = token;

            //return $"<script>window.location = 'http://127.0.0.1:5600/home/enter?token={token}'</script>";
            return "<script>window.location = 'http://127.0.0.1:5600/home/enter'</script>";
            //}
            //else
            //    return "<script>window.location = 'http://127.0.0.1:5600/user/login'</script>";
        }
    }
}
