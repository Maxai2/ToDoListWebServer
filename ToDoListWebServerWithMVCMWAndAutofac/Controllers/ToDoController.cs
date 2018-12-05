using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListWebServerWithMVCMWAndAutofac.Services;
using ToDoListWebServerWithMVCMWAndAutofac.WebServer.Attributes;

namespace ToDoListWebServerWithMVCMWAndAutofac.Controllers
{
    class ToDoController
    {
        [Authorize("User,Admin")]
        [HttpMethod("GET")]
        public string showList()
        {
            //if ((bool)data["isAuth"] == false)
            //    return "<script>window.location = 'http://127.0.0.1:5600/user/login'</script>";
            //else
            //{
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("<ol>");

                int count = 0;

                var userList = new ToDoService();

                foreach (var elem in userList.GetUserToDoList())
                {
                    strBuilder.Append($"<li>{elem.ItemName}<form method='POST' action='http://127.0.0.1:5600/toDo/changeState'> <input type='checkbox' name='check' {elem.ItemState}> <input type='hidden' name='id' value={count}> <input type='submit'> </form> </li>");
                    count++;
                }

                strBuilder.Append("</ol>");
                strBuilder.Append($"<form method='POST' action='http://127.0.0.1:5600/toDo/addToDo'> <label>To do: </label> <input type='text' name='toDoName' required> <input type='submit' value='Add'> </form>");
                strBuilder.Append("<form method='GET' action='http://127.0.0.1:5600/user/login'> <input type='submit' value='Log Out'> </form>");

                return strBuilder.ToString();
            //}
        }

        [Authorize("User,Admin")]
        [HttpMethod("POST")]
        public string addToDo(string toDoName)
        {
            var toDoS = new ToDoService();

            toDoS.addToDoItem(toDoName);
            return $"<script>window.location = 'http://127.0.0.1:5600/toDo/showList'</script>";
            //return $"<script>window.location = 'http://127.0.0.1:5600/toDo/showList?token={UserService.UserToken}'</script>";
        }

        [Authorize("Admin")]
        [HttpMethod("POST")]
        public string changeState(int id, string check)
        {
            if(UserService.ErrorMes == "")
            { 
                var toDoS = new ToDoService();

                toDoS.changeToDoState(id, check);
                return $"<script>window.location = 'http://127.0.0.1:5600/toDo/showList'</script>";
                //return $"<script>window.location = 'http://127.0.0.1:5600/toDo/showList?token={UserService.UserToken}'</script>";
            }
            else
            {
                return "<div>" +
                    $"<span style='color: red;'>{UserService.ErrorMes}</span> " +
                    "<br/>" +
                    "<a href='http://127.0.0.1:5600/toDo/showList'>Back to Login</a>" +
                    "</div>";
            }
        }
    }
}
