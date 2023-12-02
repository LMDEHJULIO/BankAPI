using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Repository.IRepository;

namespace BankAPI.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {

        private readonly ApplicationDbContext _db; 

        public CustomerRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        //public void Update(Customer entity)
        //{
        //    _db.Update(entity);
        //    _db.SaveChanges();
        //}
    }
}
