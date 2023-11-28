using AutoMapper;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase

    {

        private readonly ITransactionRepository _db;
        private readonly IAccountRepository _accountDb;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionRepository db, IAccountRepository accountDb, IMapper mapper)
        {
            _db = db;
            _accountDb = accountDb;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Transaction>> GetTransaction()
        {
            var transactions = _db.GetAll();

            //return Ok(_mapper.Map<List<BillDTO>>(bills));

            return Ok(_mapper.Map<List<TransactionDTO>>(transactions));
        }

    
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("accounts/{accountId}/transaction")]
        public ActionResult<Transaction> CreateTransaction([FromBody] TransactionDTO transactionDTO, long accountId) {

            var transaction = _mapper.Map<Transaction>(transactionDTO);

            if (transaction == null)
            {

                return BadRequest();
            }

            if (transaction.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var account = _accountDb.Get(accountId);

            account.Balance += transaction.Amount;

            if (account == null)
            {
                return NotFound("Account not Found");
            }

            transaction.AccountId = accountId;

            _db.Create(transaction);

            _db.Save();
            _accountDb.Save();

            return CreatedAtRoute("GetDeposit", new { id = transaction.Id }, transaction);
        }

        //[HttpPost("accounts/{accountId}/deposits")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public ActionResult<Account> CreateAccount([FromBody] Deposit deposit, int accountId)
        //{
        //    if (deposit == null)
        //    {

        //        return BadRequest();
        //    }

        //    if (deposit.Id > 0)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }

        //    var account = _db.Accounts.FirstOrDefault(c => c.Id == accountId);

        //    if (account == null)
        //    {
        //        return NotFound("Customer not Found");
        //    }

        //    deposit.PayeeId = accountId;

        //    _db.Deposits.Add(deposit);

        //    _db.SaveChanges();

        //    return CreatedAtRoute("GetCustomer", new { id = deposit.Id }, deposit);
        //}


    }
}
