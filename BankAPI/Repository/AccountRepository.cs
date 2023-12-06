using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Repository.IRepository;

namespace BankAPI.Repository
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly ApplicationDbContext _db;

        public AccountRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

       

        public void Update(Account entity)
        {
            _db.Update(entity);
            _db.SaveChanges();
        }
    }
}
