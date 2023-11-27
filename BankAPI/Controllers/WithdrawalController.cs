using AutoMapper;
using bankapi.models;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Controllers
{
    [Route("api/withdrawals")]
    [ApiController]
    public class WithdrawalController : ControllerBase

    {

        private readonly IWithdrawalRepository _db;
        private readonly IAccountRepository _accountDb;

        private readonly IMapper _mapper;

        public WithdrawalController(IWithdrawalRepository db, IAccountRepository accountDb, IMapper mapper)
        {
            _db = db;
            _accountDb = accountDb;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Withdrawal>> GetWithdrawals()
        {
            var withdrawal = _db.GetAll();

            //return Ok(_mapper.Map<List<BillDTO>>(bills));

            return Ok(_mapper.Map<List<WithdrawalDTO>>(withdrawal));
        }

        [HttpGet("{id}", Name = "GetWithdrawal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Withdrawal> GetWithdrawal(long id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var withdrawal = _db.Get(id);

            if (withdrawal == null)
            {
                return NotFound();
            }

            return Ok(withdrawal);
        }

        [HttpPost("accounts/{accountId}/withdrawals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Withdrawal> CreateWithdrawal([FromBody] Withdrawal withdrawal, long accountId)
        {
            if (withdrawal == null)
            {

                return BadRequest();
            }

            if (withdrawal.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var account = _accountDb.Get(accountId);

            if (account == null)
            {
                return NotFound("Customer not Found");
            }

            withdrawal.PayerId = accountId;

            _db.Create(withdrawal);

            _db.Save();

            return CreatedAtRoute("GetWithdrawal", new { id = withdrawal.Id }, withdrawal);
        }

        [HttpPut("{id}")]

        public IActionResult UpdateWithdrawal(long id, [FromBody] Withdrawal withdrawalEdit)
        {
            if (withdrawalEdit == null)
            {
                return BadRequest();
            }

            var withdrawal = _db.Get(id);

            if (withdrawal == null)
            {
                return NotFound();
            }

            withdrawal.Type = withdrawalEdit.Type;
            withdrawal.Status = withdrawalEdit.Status;
            withdrawal.Amount = withdrawalEdit.Amount;
            withdrawal.Medium = withdrawalEdit.Medium;
            withdrawal.Description = withdrawalEdit.Description;
            withdrawal.TransactionDate = withdrawalEdit.TransactionDate;
            withdrawal.PayerId = withdrawalEdit.PayerId;


            _db.Update(withdrawal);

            _db.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWithdrawal(long id)
        {
            var withdrawal = _db.Get(id);

            if (withdrawal == null)
            {
                return NotFound();
            }

            _db.Remove(withdrawal);

            _db.Save();

            return NoContent();
        }


    }
}
