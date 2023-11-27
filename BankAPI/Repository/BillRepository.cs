using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Repository.IRepository;

namespace BankAPI.Repository
{
    public class BillRepository : Repository<Bill>, IBillRepository
    {
        private readonly ApplicationDbContext _db;

        public BillRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Bill entity)
        {
            _db.Update(entity);
            _db.SaveChanges();
        }
    }
}
