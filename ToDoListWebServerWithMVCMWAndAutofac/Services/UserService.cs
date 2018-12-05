using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMVCMWAndAutofac.Services
{
    public class UserPerson
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public UserPerson(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }

    class UserService : IUserservice
    {
        private static List<UserPerson> persList = new List<UserPerson>();
        public static int UserId { get; set; }
        public static string UserToken { get; set; }
        public static string ErrorMes { get; set; }
        public static string UserRole { get; set; }

        static UserService()
        {
            persList.Add(new UserPerson("qwerty", "Qwerty"));   // User
            persList.Add(new UserPerson("qwerty1", "Qwerty1")); // Admin
            persList.Add(new UserPerson("qwerty2", "Qwerty2")); // Guest
        }

        public List<UserPerson> getUsers()
        {
            return persList;
        }
    }
}
