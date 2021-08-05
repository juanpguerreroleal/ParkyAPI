﻿using ParkyAPI.DataAccess.Repository.IRepository;
using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.DataAccess.Repository
{
    class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        private readonly ApplicationDbContext _db;
        public NationalParkRepository(ApplicationDbContext db): base (db)
        {
            _db = db;
        }

        public bool Exists(int id)
        {
            return _db.NationalParks.Any(x => x.Id == id);
        }

        public bool Exists(string name)
        {
            return _db.NationalParks.Any(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
