using BankAPI.Models;

namespace BankAPI.Repository.IRepository
{
    public interface IBillRepository : IRepository<Bill>
    {
        void Update(Bill bill);
    }
}
