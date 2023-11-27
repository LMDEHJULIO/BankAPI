using AutoMapper;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
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

        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        private readonly ILogger<BillController> _logger;

        public BillController(ApplicationDbContext db, IMapper mapper, ILogger<BillController> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<BillDTO>> GetBills()
        {
            var bills = _db.Bills.ToList();

            //return Ok(_mapper.Map<List<BillDTO>>(bills));
            return Ok(_mapper.Map<List<BillDTO>>(bills));
        }

        [HttpGet("{id}", Name = "GetBill")]
        public ActionResult<BillDTO> GetBill(int id)
        {
            var bill = _db.Bills.FirstOrDefault(b => b.Id == id);

            return Ok(_mapper.Map<BillDTO>(bill));
        }


        [HttpPost("accounts/{accountId}/bills")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BillDTO> CreateBill(int accountId, [FromBody] BillDTO billDTO)
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

            var newBill = _mapper.Map<Bill>(billDTO);

            newBill.AccountId = accountId; 

            _db.Bills.Add(newBill);


            _db.SaveChanges();

            var createdBillDto = _mapper.Map<BillDTO>(newBill);

            return CreatedAtRoute("GetBill", new { id = createdBillDto.Id }, createdBillDto);
        }
    }
}
