using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Routing;

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
        public IEnumerable<Account> GetAccounts()
        {
            return _db.Accounts;
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

    }
}
