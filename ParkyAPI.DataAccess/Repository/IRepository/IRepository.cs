using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T: class
    {
        T GetById(int id);
        T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
