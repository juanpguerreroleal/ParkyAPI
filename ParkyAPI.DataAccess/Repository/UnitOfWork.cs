using ParkyAPI.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _db;
        public INationalParkRepository NationalParkRepository { get; set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            NationalParkRepository = new NationalParkRepository(_db);
        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public bool SaveChanges()
        {
            return _db.SaveChanges() > 0;
        }
    }
}
