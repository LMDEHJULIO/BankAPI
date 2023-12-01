//using BankAPI.Models;
//using BankAPI.Models.Dto;
//using BankAPI.Models.Dto.Create;
//using BankAPI.Repository.IRepository;
//using AutoMapper;
//using System.Threading.Tasks;
//using System.Linq;

//namespace BankAPI.Services
//{

//    public class DepositService : TestService<Customer, CustomerCreateDTO, CustomerDTO>
//    {
//        private readonly IDepositRepository _depositRepository;
//        private readonly IAccountRepository _accountRepository;
//        private readonly IMapper _mapper;
//        private readonly ILogger<CustomerService> _logger;

//        public DepositService(IDepositRepository depositRepository, IAccountRepository accountRepository, IMapper mapper, ILogger<CustomerService> logger) : base(depositRepository, mapper, logger)
//        {
//            _depositRepository = depositRepository;
//            _accountRepository = accountRepository;
//            _mapper = mapper;
//            _logger = logger;
//        }


//        //public IEnumerable<Customer> GetCustomers()
//        //{
//        //    try
//        //    {
//        //        return _customerRepository.GetAll(c => c.Address);
//        //    } catch (Exception ex)
//        //    {
//        //        _logger.LogError("Error retrieving all customers" + ex.Message);
//        //        throw;
//        //    }
//        //}

//        //public Customer GetCustomer(int id)
//        //{
//        //    try
//        //    {
//        //        return _customerRepository.Get(id, c => c.Address);
//        //    } catch (Exception ex)
//        //    {
//        //        _logger.LogError("Error retreiving customer" + ex.Message);
//        //        throw;
//        //    }
//        //}

//        public IEnumerable<AccountDTO> GetCustomerAccounts(int customerId)
//        {
//            try
//            {
//                var customer = _depositRepository.Get(customerId);

//                if (customer == null)
//                {
//                    return null;
//                }

//                var accounts = _accountRepository.GetAll().Where(a => a.Id == customerId);

//                return _mapper.Map<List<AccountDTO>>(accounts);

//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Error fetching accounts: " + ex.Message);

//                throw;

//            }
//        }

//        //public Customer CreateCustomer(CustomerCreateDTO customerDTO)
//        //{
//        //    try
//        //    {
//        //        var customer = _mapper.Map<Customer>(customerDTO);
//        //        _customerRepository.Create(customer);
//        //        _customerRepository.Save();

//        //        return customer;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _logger.LogError("Error creating customer: " + ex.Message);
//        //        throw;
//        //    }
//        //}

//        //public bool DeleteCustomer(int id)
//        //{
//        //    try
//        //    {
//        //        var customer = _customerRepository.Get(id);

//        //        if (customer == null)
//        //        {
//        //            return false;
//        //        }

//        //        _customerRepository.Remove(customer);
//        //        _customerRepository.Save();

//        //        return true;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _logger.LogError("Error deleting customer: " + ex.Message);
//        //        throw;
//        //    }
//        //}

//        public Customer UpdateCustomer(int id, CustomerDTO customerDTO)
//        {
//            try
//            {
//                var editCustomer = _customerRepository.Get(id);

//                if (editCustomer == null)
//                {
//                    return null;
//                }

//                _mapper.Map(customerDTO, editCustomer);
//                _customerRepository.Update(editCustomer);
//                _customerRepository.Save();

//                return editCustomer;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Error updating customer: " + ex.Message);
//                throw;
//            }
//        }


//    }




//}