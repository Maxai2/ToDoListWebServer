using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMiddleWare.Services
{
    class toDoItem
    {
        public string itemName { get; set; }
        public string itemState { get; set; }

        public toDoItem(string itemName)
        {
            this.itemName = itemName;
            this.itemState = "";
        }
    }

    class ToDoService
    {
        private static List<toDoItem> toDoList = new List<toDoItem>();

        public List<toDoItem> GetToDoList()
        {
            return toDoList;
        }

        public void addToDoItem(string itemName)
        {
            toDoList.Add(new toDoItem(itemName));
        }

        public void changeToDoState(int index, string check)
        {
            if (check == "on")
                toDoList[index].itemState = "checked";
            else
                toDoList[index].itemState = "";
        }
    }
}
