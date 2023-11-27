using bankapi.models;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Repository.IRepository;

namespace BankAPI.Repository
{
    public class WithdrawalRepository : Repository<Withdrawal>, IWithdrawalRepository
    {
        private readonly ApplicationDbContext _db;

        public WithdrawalRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Withdrawal entity)
        {
            _db.Update(entity);
            _db.SaveChanges();
        }
    }
}
