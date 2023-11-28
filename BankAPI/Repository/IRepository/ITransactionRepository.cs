using BankAPI.Models;

namespace BankAPI.Repository.IRepository
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        void Update(Transaction transaction);

    }
}
