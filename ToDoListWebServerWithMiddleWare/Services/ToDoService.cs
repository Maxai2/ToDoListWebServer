using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMiddleWare.Services
{
    class ToDoItem
    {
        public string ItemName { get; set; }
        public string ItemState { get; set; }

        public ToDoItem(string ItemName)
        {
            this.ItemName = ItemName;
            this.ItemState = "";
        }
    }

    class ToDoService
    {
        private static Dictionary<int, List<ToDoItem>> userToDoList = new Dictionary<int, List<ToDoItem>>();

        static ToDoService()
        {
            userToDoList.Add(0, new List<ToDoItem>());
            userToDoList.Add(1, new List<ToDoItem>());
            userToDoList.Add(2, new List<ToDoItem>());
        }

        public List<ToDoItem> GetUserToDoList()
        {
            return userToDoList[key: UserService.UserId];
        }

        public void addToDoItem(string ItemName)
        {
            userToDoList[key: UserService.UserId].Add(new ToDoItem(ItemName));
        }

        public void changeToDoState(int index, string check)
        {
            if (check == "on")
                userToDoList[key: UserService.UserId][index].ItemState = "checked";
            else
                userToDoList[key: UserService.UserId][index].ItemState = "";
        }
    }
}
