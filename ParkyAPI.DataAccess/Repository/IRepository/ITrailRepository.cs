using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.DataAccess.Repository.IRepository
{
    public interface ITrailRepository: IRepository<Trail>
    {
        bool Exists(int id);
        bool Exists(string name);
    }
}
