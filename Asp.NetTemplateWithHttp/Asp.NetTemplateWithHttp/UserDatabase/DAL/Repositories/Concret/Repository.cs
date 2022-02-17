using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserDatabase.DAL.Repositories.Abstract;

namespace UserDatabase.DAL.Repositories.Concret
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {

        private SQLiteConnection db;

        public Repository(SQLiteConnection db)
        {
            this.db = db;
        }

        public TableQuery<T> AsQueryable() =>
            db.Table<T>();

        public List<T> Get() =>
             db.Table<T>().ToList();


        public T Get(int id) =>
               db.Find<T>(id);

        public T Get(Expression<Func<T, bool>> predicate) =>
           db.Find<T>(predicate);

        public void Insert(T entity) =>
               db.Insert(entity);

        public void Update(T entity) =>
               db.Update(entity);

        public void Delete(T entity) =>
               db.Delete(entity);
    }
}
