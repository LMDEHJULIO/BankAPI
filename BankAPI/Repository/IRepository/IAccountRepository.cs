using BankAPI.Models;

namespace BankAPI.Repository.IRepository
{
    public interface IAccountRepository : IRepository<Account>
    {
        void Update(Account account);
    }
}
