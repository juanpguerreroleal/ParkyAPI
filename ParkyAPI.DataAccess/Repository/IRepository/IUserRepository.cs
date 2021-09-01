using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.DataAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        User Authenticate(string username, string password);
        User Register(string username, string password);
    }
}
