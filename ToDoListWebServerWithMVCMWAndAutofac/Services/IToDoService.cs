using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMVCMWAndAutofac.Services
{
    public interface IToDoService
    {
        List<ToDoItem> GetUserToDoList();
        void addToDoItem(string ItemName);
        void changeToDoState(int index, string check);
    }
}
