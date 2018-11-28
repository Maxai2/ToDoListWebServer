using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWebServerWithMVCMWAndAutofac.Services
{
    public interface IUserservice
    {
        List<UserPerson> getUsers();
    }
}
