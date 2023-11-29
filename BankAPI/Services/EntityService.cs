using BankAPI.Repository.IRepository;
using System.Linq.Expressions;

namespace BankAPI.Services
{
    public class EntityService<T> : IEntityService<T> where T : class
    {
        private readonly IRepository<T> _repository;
        
        public delegate Expression<Func<T, object>> IncludeProperty<T>(T entity);

        public EntityService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public T GetById<TKey>(TKey id, params Expression<Func<T, object>>[] includeProperties) {
            return _repository.Get(id, includeProperties);
        }

        public bool Exists(Expression<Func<T, bool>> filter = null) {
            return _repository.Any(filter);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> filter) {
            return _repository.Where(filter);
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProperties) {
            return _repository.GetAll(includeProperties);
        }

        public IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties) { 
            return _repository.GetAllIncluding(includeProperties);
        }

        public void Create(T entity) {
            _repository.Create(entity);
        }

        public void Remove(T entity) {
            _repository.Remove(entity);
        }

        public bool SaveChanges() {
            return _repository.Save();
        }
    }
}
