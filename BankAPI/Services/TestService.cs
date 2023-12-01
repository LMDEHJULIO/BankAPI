using AutoMapper;
using BankAPI.Repository.IRepository;
using System.Linq.Expressions;

namespace BankAPI.Services
{
    public abstract class TestService<TEntity, TCreateDTO, TUpdateDTO>
           where TEntity : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger<TestService<TEntity, TCreateDTO, TUpdateDTO>> _logger;

        protected TestService(IRepository<TEntity> repository, IMapper mapper, ILogger<TestService<TEntity, TCreateDTO, TUpdateDTO>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public virtual IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                return _repository.GetAll(includeProperties);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }

        public virtual TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                return _repository.Get(id, includeProperties);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }

        public virtual TDTO Create<TDTO>(TCreateDTO createDTO)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(createDTO);
                _repository.Create(entity);
                _repository.Save();
                return _mapper.Map<TDTO>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }



        //public virtual bool update(int id, tupdatedto updatedto)
        //{
        //    try
        //    {
        //        var entity = _repository.get(id);
        //        if (entity == null)
        //        {
        //            return false;
        //        }

        //        _mapper.map(updatedto, entity);
        //        _repository.update(entity);
        //        _repository.save();
        //        return true;
        //    }
        //    catch (exception ex)
        //    {
        //        _logger.logerror($"error updating {typeof(tentity).name}: {ex.message}");
        //        throw;
        //    }
        //}

        public virtual bool Delete(int id)
        {
            try
            {
                var entity = _repository.Get(id);
                if (entity == null)
                {
                    return false;
                }

                _repository.Remove(entity);
                _repository.Save();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting {typeof(TEntity).Name}: {ex.Message}");
                throw;
            }
        }
    }
}
