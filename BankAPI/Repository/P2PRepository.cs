using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Repository.IRepository;

namespace BankAPI.Repository
{
    public class P2PRepository : Repository<P2P>, IP2PRepository
    {
        private readonly ApplicationDbContext _db;

        public P2PRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(P2P entity)
        {
            _db.Update(entity);
            _db.SaveChanges();
        }
    }
}
