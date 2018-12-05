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
                    "</form>";// +
                    //$"<span style='color: red;'>{UserService.ErrorMes}</span> ";
        }
        
        [HttpMethod("POST")]
        public string checkLogin(string login, string password)
        {
            var userS = new UserService();

            var userIndex = userS.getUsers().FindIndex(u => (u.Name == login) && (u.Password == password));

            if (userIndex != -1)
            {
                UserService.UserId = userIndex;

                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();

                string token = Convert.ToBase64String(time.Concat(key).ToArray());
                UserService.UserToken = token;

                switch (userIndex)
                {
                    case 0:
                        UserService.UserRole = "User";
                        break;
                    case 1:
                        UserService.UserRole = "Admin";
                        break;
                    case 3:
                        UserService.UserRole = "Guest";
                        break;
                }


                return $"<script>window.location = 'http://127.0.0.1:5600/home/enter'</script>";
                //return $"<script>window.location = 'http://127.0.0.1:5600/home/enter?token={token}&role={UserService.UserRole}'</script>";
            }
            else
                return "<script>window.location = 'http://127.0.0.1:5600/user/login'</script>";

            //if (UserService.ErrorMes == "")
            //{
            //    return "<script>window.location = 'http://127.0.0.1:5600/home/enter'</script>";
            //}
            //else
            //{
            //    return "<div>" +
            //        $"<span style='color: red;'>{UserService.ErrorMes}</span> " +
            //        "<br/>" +
            //        "<a href='http://127.0.0.1:5600/user/login'>Back to Login</a>" +
            //        "</div>";
            //}
        }
    }
}
