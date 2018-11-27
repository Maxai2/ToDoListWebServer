using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebServerWithMiddleWare.Services;

namespace ToDoListWebServerWithMVCMiddleWare.Controllers
{
    class UserController
    {
        private Dictionary<string, object> data;

        public UserController(Dictionary<string, object> data)
        {
            this.data = data;
        }

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
                   "</form>";
        }

        public string checkLogin(string login, string password)
        {
            var userIndex = UserService.getUsers().FindIndex(u => (u.Name == login) && (u.Password == password));

            if (userIndex != -1)
            {
                UserService.UserId = userIndex;

                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();

                string token = Convert.ToBase64String(time.Concat(key).ToArray());
                UserService.UserToken = token;

                return $"<script>window.location = 'http://127.0.0.1:5600/home/enter?token={token}'</script>";
            }
            else
                return "<script>window.location = 'http://127.0.0.1:5600/user/login'</script>";
        }
    }
}
