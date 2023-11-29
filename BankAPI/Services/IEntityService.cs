using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;

namespace BankAPI.Services
{
    public interface IEntityService<T> where T : class
    {
        T GetById<TKey>(TKey id, params Expression<Func<T, object>>[] includeProperties);

        bool Exists(Expression<Func<T, bool>> filter = null);

        IEnumerable<T> Find(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        void Create(T entity);

        void Remove(T entity);

        bool SaveChanges(); 


    }
}
