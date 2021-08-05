using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        INationalParkRepository NationalParkRepository { get; }
        bool SaveChanges();
    }
}
