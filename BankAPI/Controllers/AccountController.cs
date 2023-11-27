using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _db;
        private readonly ICustomerRepository _customerDb;

        public AccountController(IAccountRepository db, ICustomerRepository customerDb)
        {
            _db = db;
            _customerDb = customerDb;
        }

        [HttpGet]
        public IEnumerable<Account> GetAccounts()
        {
            return _db.GetAll(); 
        }

        [HttpGet("customers/{customerId}/accounts")]
        public ActionResult<ICollection<Account>> GetAccounts(int customerId)
        {
            var customerValid = _customerDb.Any(c => c.Id == customerId);

            if(!customerValid)
            {
                return NotFound();
            }

            var accounts = _db.Include(a => a.Customer).Where(a => a.CustomerId == customerId).ToList();

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

            var customer = _db.Any(c => c.Id == customerId);

            if(customer == false)
            {
                return NotFound("Customer not Found");
            }

            account.CustomerId = customerId;

            _db.Create(account);

            _db.Save();

            return CreatedAtRoute("GetCustomer", new { id = account.Id }, account);
        }

        [HttpPut("{id}")]

        public IActionResult UpdateAccount(long id, [FromBody] Account accountEdit)
        {
            if (accountEdit == null)
            {
                return BadRequest();
            }

            var account = _db.Get(id);

            if (account == null) 
            {
                return NotFound();
            }

            account.Type = accountEdit.Type;
            account.Rewards = accountEdit.Rewards;
            account.Balance = accountEdit.Balance;
            account.NickName = accountEdit.NickName;

            _db.Update(account);

            _db.Save();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(long id)
        {
            var account = _db.Get(id);

            if (account == null)
            {
                return NotFound();
            }

            _db.Remove(account);

            _db.Save();

            return NoContent();
        }



    }
}
