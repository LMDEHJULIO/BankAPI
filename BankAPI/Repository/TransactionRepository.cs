using BankAPI.Data;
using bankapi.models;
using BankAPI.Models;
using BankAPI.Repository.IRepository;

namespace BankAPI.Repository
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _db;

        public TransactionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Transaction entity)
        {
            _db.Update(entity);
            _db.SaveChanges();
        }
    }
}
