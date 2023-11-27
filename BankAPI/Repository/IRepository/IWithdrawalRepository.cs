using bankapi.models;
using BankAPI.Models;

namespace BankAPI.Repository.IRepository
{
    public interface IWithdrawalRepository : IRepository<Withdrawal>
    {
        void Update(Withdrawal withdrawal);
    }
}
