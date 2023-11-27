using BankAPI.Data;
using BankAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.CRUD;
using System.Linq.Expressions;

namespace BankAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db; 
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }


        public T Get<TKey>(TKey id, params Expression<Func<T, object>>[] includeProperties) {
            IQueryable<T> query = dbSet;
            foreach (var includeProperty in includeProperties) { 
                query = query.Include(includeProperty);
            }

            return query.FirstOrDefault(e => EF.Property<TKey>(e, "Id").Equals(id));
        }

        public bool Any(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            return query.Any();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> filter) {
        
            IQueryable<T> query = dbSet;

            if (filter != null) {
                query = query.Where(filter); 
            }

            return query.ToList();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }

        public IQueryable<T> Include(Expression<Func<T, object>> includeExpresion)
        {
            return dbSet.Include(includeExpresion);
        }

        public IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.ToList();
        }

        public void Create(T entity) {
            dbSet.Add(entity);
            Save();
        }



        public void Remove(T entity) {
            dbSet.Remove(entity);
            Save();
        }

        public bool Save() { 
            return _db.SaveChanges() >= 0;
        }
    }
}
