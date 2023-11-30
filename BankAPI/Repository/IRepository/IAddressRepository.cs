using BankAPI.Models;

namespace BankAPI.Repository.IRepository
{
    public interface IAddressRepository : IRepository<Deposit>
    {
        void Update(Deposit deposit);
    }
}
