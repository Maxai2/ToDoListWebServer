using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMiddleWare.Services
{
    class UserPerson
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public UserPerson(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }

    class UserService
    {
        private static List<UserPerson> persList = new List<UserPerson>();
        public static int UserId { get; set; }
        public static string UserToken { get; set; }

        static UserService()
        {
            persList.Add(new UserPerson("qwerty", "Qwerty"));
            persList.Add(new UserPerson("qwerty1", "Qwerty1"));
            persList.Add(new UserPerson("qwerty2", "Qwerty2"));
        }

        public static List<UserPerson> getUsers()
        {
            return persList;
        }
    }
}
