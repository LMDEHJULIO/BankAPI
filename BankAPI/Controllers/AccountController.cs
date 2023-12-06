using AutoMapper;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Models.Dto.Create;
using BankAPI.Models.Dto.Update;
using BankAPI.Repository.IRepository;
using BankAPI.Services;
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
        private readonly IMapper _mapper;
        private readonly AccountService _accountService;

        public AccountController(IMapper mapper, IAccountRepository db, ICustomerRepository customerDb, AccountService acccountService)
        {
            _db = db;
            _customerDb = customerDb;
            _mapper = mapper;
            _accountService = acccountService;
        }

        [HttpGet]
        public ActionResult<APIResponse> GetAccounts()
        {
            var accounts = _mapper.Map<IEnumerable<AccountDTO>>(_accountService.GetAllAccounts());
            return Ok(new APIResponse(System.Net.HttpStatusCode.OK, accounts, "Success"));
        }

        [HttpGet("customers/{customerId}/accounts")]
        public ActionResult<APIResponse> GetAccountsById(int customerId)
        {
            if (!_customerDb.Any(c => c.Id == customerId))
            {
                return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Customer not found"));
            }

            var accounts = _db.Include(a => a.Customer.Address).Where(a => a.CustomerId == customerId).ToList();
            return Ok(new APIResponse(System.Net.HttpStatusCode.OK, accounts, "Success"));
        }

        [HttpPost("customers/{customerId}/accounts")]
        public ActionResult<APIResponse> CreateAccount([FromBody] AccountCreateDTO accountInfo, int customerId)
        {
            if (accountInfo == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid account data"));
            }

            if (accountInfo.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Invalid account ID"));
            }

            if (!_customerDb.Any(c => c.Id == customerId))
            {
                return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Customer not found"));
            }

            accountInfo.CustomerId = customerId;
            var account = _mapper.Map<Account>(accountInfo);
            _accountService.CreateAccount(account);

            return CreatedAtRoute("GetCustomer", new { id = account.Id }, new APIResponse(System.Net.HttpStatusCode.Created, account, "Account created successfully"));
        }

        [HttpPut("{id}")]
        public ActionResult<APIResponse> UpdateAccount(long id, [FromBody] AccountUpdateDTO accountEdit)
        {
            if (accountEdit == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid account data"));
            }

            if (_db.Get(id) == null)
            {
                return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Account not found"));
            }

            _accountService.UpdateAccount(id, _mapper.Map<Account>(accountEdit));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<APIResponse> DeleteAccount(long id)
        {
            if (_accountService.GetAccountById(id) == null)
            {
                return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Account not found"));
            }

            _accountService.DeleteAccount(id);
            return Ok(new APIResponse(System.Net.HttpStatusCode.NoContent, "Account successfully deleted"));
        }



    }
}
