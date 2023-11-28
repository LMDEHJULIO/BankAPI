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
        public ActionResult<APIResponse> GetBills()
        {
            try
            {
                var bills = _db.GetAll();
                var billDTOs = _mapper.Map<List<BillDTO>>(bills);
                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, billDTOs, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, ex.Message));
            }
        }

        [HttpGet("{id}", Name = "GetBill")]
        public ActionResult<APIResponse> GetBill(int id)
        {
            try
            {
                var bill = _db.Get(id);
                if (bill == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Bill not found"));
                }

                var billDTO = _mapper.Map<BillDTO>(bill);
                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, billDTO, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, ex.Message));
            }
        }

        [HttpPost("accounts/{accountId}/bills")]
        public ActionResult<APIResponse> CreateBill(long accountId, [FromBody] BillDTO billDTO)
        {
            if (billDTO == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Bill data is null"));
            }

            try
            {
                if (!_accountDb.Any(a => a.Id == accountId))
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Account not found"));
                }

                var newBill = _mapper.Map<Bill>(billDTO);
                newBill.AccountId = accountId;
                _db.Create(newBill);
                _db.Save();

                var createdBillDto = _mapper.Map<BillDTO>(newBill);
                return CreatedAtRoute("GetBill", new { id = createdBillDto.Id }, new APIResponse(System.Net.HttpStatusCode.Created, createdBillDto, "Bill created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error creating bill: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public ActionResult<APIResponse> UpdateBill(int id, [FromBody] BillDTO billDTO)
        {
            if (billDTO == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid bill data"));
            }

            try
            {
                var bill = _db.Get(id);
                if (bill == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Bill not found"));
                }

                _mapper.Map(billDTO, bill);
                _db.Update(bill);
                _db.Save();

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, _mapper.Map<BillDTO>(bill), "Bill updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error updating bill: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<APIResponse> DeleteBill(int id)
        {
            try
            {
                var bill = _db.Get(id);
                if (bill == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Bill not found"));
                }

                _db.Remove(bill);
                _db.Save();

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, null, "Bill deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error deleting bill: " + ex.Message));
            }
        }
    }


}
