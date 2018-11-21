using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMiddleWare.Services
{
    class userPerson
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public userPerson(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }

    class UserService
    {
        private static List<userPerson> persList = new List<userPerson>();
        public static int UserId { get; set; }
        public static string UserToken { get; set; }

        static UserService()
        {
            persList.Add(new userPerson("qwerty", "Qwerty"));
            persList.Add(new userPerson("qwerty1", "Qwerty1"));
            persList.Add(new userPerson("qwerty2", "Qwerty2"));
        }

        public static List<userPerson> getUsers()
        {
            return persList;
        }
    }
}
