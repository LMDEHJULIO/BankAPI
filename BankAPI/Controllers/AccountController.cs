using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ICollection<Account> GetAccounts()
        {
            return _db.Accounts.Include(a => a.Customer).ThenInclude(c => c.Address).ToList(); 
        }

        [HttpGet("customers/{customerId}/accounts")]
        public ActionResult<ICollection<Account>> GetAccounts(int customerId)
        {
            var customerValid = _db.Customers.Any(c => c.Id == customerId);

            if(!customerValid)
            {
                return NotFound();
            }

            var accounts = _db.Accounts.Where(a => a.CustomerId == customerId).Include(a => a.Customer).ThenInclude(c => c.Address).ToList(); 

            return Ok(accounts);
        }

        [HttpPost("customers/{customerId}/accounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Account> CreateAccount([FromBody] Account account, int customerId)
        {
            if (account == null)
            {

                return BadRequest();
            }

            if (account.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var customer = _db.Customers.FirstOrDefault(c => c.Id == customerId);

            if(customer == null)
            {
                return NotFound("Customer not Found");
            }

            account.CustomerId = customerId;

            _db.Accounts.Add(account);

            _db.SaveChanges();

            return CreatedAtRoute("GetCustomer", new { id = account.Id }, account);
        }

        [HttpPut("{id}")]

        public IActionResult UpdateAccount(long id, [FromBody] Account accountEdit)
        {
            if (accountEdit == null)
            {
                return BadRequest();
            }

            var account = _db.Accounts.FirstOrDefault(a => a.Id == id);

            if (account == null) 
            {
                return NotFound();
            }

            account.Type = accountEdit.Type;
            account.Rewards = accountEdit.Rewards;
            account.Balance = accountEdit.Balance;
            account.NickName = accountEdit.NickName;

            _db.Accounts.Update(account);

            _db.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(long id)
        {
            var account = _db.Accounts.FirstOrDefault(a =>a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            _db.Accounts.Remove(account);

            _db.SaveChanges();

            return NoContent();
        }



    }
}
