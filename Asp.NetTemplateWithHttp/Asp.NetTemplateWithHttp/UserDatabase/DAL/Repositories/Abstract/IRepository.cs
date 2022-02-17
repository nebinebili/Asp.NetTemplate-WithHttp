using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserDatabase.DAL.Repositories.Abstract
{
    public interface IRepository<T> where T : class, new()
    {
        List<T> Get();
        T Get(int id);
        T Get(Expression<Func<T, bool>> predicate);
        TableQuery<T> AsQueryable();
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
