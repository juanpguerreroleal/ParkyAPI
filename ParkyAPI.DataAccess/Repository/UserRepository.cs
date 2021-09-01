using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.DataAccess.Repository
{
    class UserRepository : IRepository.IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public User Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool IsUniqueUser(string username)
        {
            throw new NotImplementedException();
        }

        public User Register(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
