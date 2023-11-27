using AutoMapper;
using bankapi.models;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.NetworkInformation;
using System.Numerics;

namespace BankAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {

        private readonly IBillRepository _db;

        private readonly IAccountRepository _accountDb;

        private readonly IMapper _mapper;

        private readonly ILogger<BillController> _logger;

        public BillController(IBillRepository db, IAccountRepository accountDb, IMapper mapper, ILogger<BillController> logger)
        {
            _db = db;
            _accountDb = accountDb;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<BillDTO>> GetBills()
        {
            var bills = _db.GetAll();

            //return Ok(_mapper.Map<List<BillDTO>>(bills));
            return Ok(_mapper.Map<List<BillDTO>>(bills));
        }

        [HttpGet("{id}", Name = "GetBill")]
        public ActionResult<BillDTO> GetBill(int id)
        {
            var bill = _db.Get(id);

            return Ok(_mapper.Map<BillDTO>(bill));
        }


        [HttpPost("accounts/{accountId}/bills")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BillDTO> CreateBill(long accountId, [FromBody] BillDTO billDTO)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            //if (bill.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}

            _logger.LogInformation("Creating bill for accountId: {AccountId}", accountId);

            if (!_accountDb.Any(a => a.Id == accountId)) {
                return NotFound("Account not found");
            }

            var newBill = _mapper.Map<Bill>(billDTO);

            newBill.AccountId = accountId; 

            _db.Create(newBill);


            _db.Save();

            var createdBillDto = _mapper.Map<BillDTO>(newBill);

            return CreatedAtRoute("GetBill", new { id = createdBillDto.Id }, createdBillDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBill(int id, [FromBody] Bill billEdit)
        {
            if (billEdit == null)
            {
                return BadRequest();
            }

            var bill = _db.Get(id);

            if (bill == null)
            {
                return NotFound();
            }

            bill.Id = billEdit.Id;           
            bill.Status = billEdit.Status;
            bill.Payee = billEdit.Payee;           
            bill.Nickname = billEdit.Nickname;
            bill.CreationDate = billEdit.CreationDate;
            bill.PaymentDate = billEdit.PaymentDate;
            bill.RecurringDate = billEdit.RecurringDate;
            bill.UpcomingPaymentDate = billEdit.UpcomingPaymentDate;
            bill.PaymentAmount = billEdit.PaymentAmount;
            bill.AccountId = billEdit.AccountId;
            bill.Account = billEdit.Account;


            _db.Update(bill);

            _db.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBill(int id)
        {
            var bill = _db.Get(id);

            if (bill == null)
            {
                return NotFound();
            }

            _db.Remove(bill);

            _db.Save();

            return NoContent();
        }
    }


}
