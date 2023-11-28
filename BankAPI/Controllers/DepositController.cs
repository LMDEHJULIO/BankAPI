using AutoMapper;
using bankapi.models;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Route("api/deposits")]
    [ApiController]
    public class DepositController : ControllerBase

    {

        private readonly IDepositRepository _db;
        private readonly IAccountRepository _accountDb;

        private readonly IMapper _mapper;

        public DepositController(IDepositRepository db, IAccountRepository accountDb, IMapper mapper)
        {
            _db = db;
            _accountDb = accountDb;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Deposit>> GetDeposits()
        {
            var deposits = _db.GetAll().Where(d => d.Type == TransactionType.Deposit);

            //return Ok(_mapper.Map<List<BillDTO>>(bills));

            return Ok(_mapper.Map<List<DepositDTO>>(deposits));
        }

        [HttpGet("{id}", Name = "GetDeposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Deposit> GetDeposit(long id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var deposit = _db.Get(id);

            if (deposit == null)
            {
                return NotFound();
            }

            return Ok(deposit);
        }


        [HttpPost("accounts/{accountId}/deposits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Account> CreateDeposit([FromBody] DepositDTO depositDTO, long accountId)
        {

            var deposit = _mapper.Map<Deposit>(depositDTO);

            if (deposit == null)
            {

                return BadRequest();
            }

            if (deposit.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var account = _accountDb.Get(accountId);

            account.Balance += deposit.Amount;

            if (account == null)
            {
                return NotFound("Account not Found");
            }

            deposit.AccountId = accountId;

            _db.Create(deposit);

            _db.Save();
            _accountDb.Save();

            return CreatedAtRoute("GetDeposit", new { id = deposit.Id }, deposit);
        }


        [HttpPut("{id}")]

        public IActionResult UpdateDeposit(long id, [FromBody] Deposit depositEdit)
        {
            if (depositEdit == null)
            {
                return BadRequest();
            }

            var deposit = _db.Get(id);

            if (deposit == null)
            {
                return NotFound();
            }

            deposit.Type = depositEdit.Type;
            deposit.Status = depositEdit.Status;
            deposit.Amount = depositEdit.Amount;
            deposit.Medium = depositEdit.Medium;
            deposit.Description = depositEdit.Description;
            deposit.TransactionDate = depositEdit.TransactionDate;
            deposit.AccountId = depositEdit.AccountId;


            _db.Update(deposit);

            _db.Save();

            return NoContent();
        }




        [HttpDelete("{id}")]
        public IActionResult DeleteDeposit(long id)
        {
            var deposit = _db.Get(id);

            if (deposit == null)
            {
                return NotFound();
            }

            _db.Remove(deposit);

            _db.Save();

            return NoContent();
        }


    }
}
