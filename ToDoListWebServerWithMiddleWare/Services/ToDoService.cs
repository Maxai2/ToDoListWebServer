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

        static ToDoService()
        {
            userToDoList.Add('0', new List<toDoItem>());
            userToDoList.Add('1', new List<toDoItem>());
            userToDoList.Add('2', new List<toDoItem>());
        }

        public List<toDoItem> GetUserToDoList()
        {
            return userToDoList[key: UserService.UserId];
        }

        public void addToDoItem(string ItemName)
        {
            userToDoList[key: UserService.UserId].Add(new toDoItem(ItemName));
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
