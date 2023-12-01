using AutoMapper;
using BankAPI.Models;
using BankAPI.Repository;
using BankAPI.Repository.IRepository;

namespace BankAPI.Services
{
    public class ICustomerService : EntityService<Customer>
    {
        public ICustomerService(IRepository<Customer> repository) : base(repository) {
        }
    }
}
