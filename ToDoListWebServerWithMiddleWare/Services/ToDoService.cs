using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMiddleWare.Services
{
    class toDoItem
    {
        public string ItemName { get; set; }
        public string ItemState { get; set; }

        public toDoItem(string ItemName)
        {
            this.ItemName = ItemName;
            this.ItemState = "";
        }
    }

    class ToDoService
    {
        private static Dictionary<int, List<toDoItem>> userToDoList = new Dictionary<int, List<toDoItem>>();

        static UserService userService = new UserService();

        public List<toDoItem> GetUserToDoList()
        {
            return userToDoList[key: userService.UserId];
        }

        public void addToDoItem(string ItemName)
        {
            userToDoList[key: userService.UserId].Add(new toDoItem(ItemName));
        }

        public void changeToDoState(int index, string check)
        {
            if (check == "on")
                userToDoList[key: userService.UserId][index].ItemState = "checked";
            else
                userToDoList[key: userService.UserId][index].ItemState = "";
        }
    }
}
