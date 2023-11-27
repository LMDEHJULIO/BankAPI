using BankAPI.Models;

namespace BankAPI.Repository.IRepository
{
    public interface IDepositRepository :  IRepository<Deposit>
    {
        void Update(Deposit deposit);
    }
}
