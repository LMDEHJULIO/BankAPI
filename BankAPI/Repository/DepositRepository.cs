using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Repository.IRepository;

namespace BankAPI.Repository
{
    public class DepositRepository : Repository<Deposit>, IDepositRepository
    {
        private readonly ApplicationDbContext _db;

        public DepositRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Deposit entity)
        {
            _db.Update(entity);
            _db.SaveChanges();
        }
    }
}
