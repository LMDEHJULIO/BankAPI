using AutoMapper;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Route("api/deposits")]
    [ApiController]
    public class DepositController : ControllerBase

    {

        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        public DepositController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Deposit>> GetDeposits()
        {
            var deposits = _db.Deposits.ToList();

            //return Ok(_mapper.Map<List<BillDTO>>(bills));

            return Ok(_mapper.Map<List<DepositDTO>>(deposits));
        }

        [HttpPost("accounts/{accountId}/deposits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Account> CreateAccount([FromBody] Deposit deposit, int accountId)
        {
            if (deposit == null)
            {

                return BadRequest();
            }

            if (deposit.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var account = _db.Accounts.FirstOrDefault(c => c.Id == accountId);

            if (account == null)
            {
                return NotFound("Customer not Found");
            }

            deposit.PayeeId = accountId;

            _db.Deposits.Add(deposit);

            _db.SaveChanges();

            return CreatedAtRoute("GetCustomer", new { id = deposit.Id }, deposit);
        }


    }
}
