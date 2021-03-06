using Microsoft.Extensions.Options;
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
        public ITrailRepository TrailRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public UnitOfWork(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            NationalParkRepository = new NationalParkRepository(_db);
            TrailRepository = new TrailRepository(_db);
            UserRepository = new UserRepository(_db, appSettings);
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
