using AutoMapper;
using BankAPI.Models;
using BankAPI.Repository.IRepository;

namespace BankAPI.Services
{




    public class CustomerService
    {
        private readonly ICustomerRepository _db;
        private readonly IAccountRepository _accountDb;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        private T Execute<T>(Func<T> action, string actionName) {
            try {
                return action();
            } catch (Exception ex) {
                _logger.LogError($"Error during {actionName}: {ex.Message}");
                throw;
            }
        }

        public CustomerService(ICustomerRepository db, IAccountRepository accountDb, IMapper mapper, ILogger<CustomerService> logger)
        {
            _db = db;
            _accountDb = accountDb;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<Customer> GetAllCustomers() {

            return Execute(() => _db.GetAll(c => c.Address), "GetAllCustomers");

        }

        public Customer GetCustomerById(int id)
        {

            return Execute(() => _db.Get(id, c => c.Address), $"GetCustomerById for ID {id}");

        }

        public IEnumerable<Account> GetCustomerAccounts(int id) {

            return Execute(() => _accountDb.GetAll().Where(a => a.CustomerId == id).ToList(), $"GetCustomerById for ID {id}");

        } 
    }
}
