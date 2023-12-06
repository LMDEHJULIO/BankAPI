using BankAPI.Models;
using BankAPI.Repository.IRepository;

namespace BankAPI.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;

        public CustomerService(ICustomerRepository customerRepository, IAccountRepository accountRepository)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAll(c => c.Address);
        }

        public Customer GetCustomer(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID");
            }

            return _customerRepository.Get(id, c => c.Address);
        }

        public Customer CreateCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            _customerRepository.Create(customer);
            _customerRepository.Save();
            return customer;
        }

        public Customer UpdateCustomer(long id, Customer customer)
        {
            if (id <= 0 || customer == null)
            {
                throw new ArgumentException("Invalid input");
            }

            var existingCustomer = _customerRepository.Get(id);
            if (existingCustomer == null)
            {
                return null;
            }

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            // Update other properties as needed

            _customerRepository.Update(existingCustomer);
            _customerRepository.Save();
            return existingCustomer;
        }

        public bool DeleteCustomer(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID");
            }

            var customer = _customerRepository.Get(id);
            if (customer == null)
            {
                return false;
            }

            _customerRepository.Remove(customer);
            _customerRepository.Save();
            return true;
        }
    }
}
