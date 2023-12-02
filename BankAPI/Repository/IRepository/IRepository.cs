using System.Linq.Expressions;

namespace BankAPI.Repository.IRepository
{
    public interface IRepository<T> { 
        T Get<TKey>(TKey id, params Expression<Func<T, object>>[] includeProperties);

        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);

        IEnumerable<T> Where(Expression<Func<T, bool>> filter);

        IQueryable<T> Include(Expression<Func<T, object>> includeExpression); 

        IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        bool Any(Expression<Func<T, bool>> filter = null);

        void Create(T entity); 

        void Update(T entity);

        void Remove(T entity);

        bool Save();
    }
}
