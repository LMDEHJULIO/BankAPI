using AutoMapper;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly BillService _billService;
        private readonly IMapper _mapper;

        public BillController(BillService billService, IMapper mapper)
        {
            _billService = billService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<APIResponse> GetBills()
        {
            try
            {
                var bills = _billService.GetBills();
                var billDTOs = _mapper.Map<List<BillDTO>>(bills);
                return Ok(new APIResponse(HttpStatusCode.OK, billDTOs, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(HttpStatusCode.InternalServerError, null, ex.Message));
            }
        }

        [HttpGet("{id}", Name = "GetBill")]
        public ActionResult<APIResponse> GetBill(int id)
        {
            try
            {
                var bill = _billService.GetBillById(id);
                if (bill == null)
                {
                    return NotFound(new APIResponse(HttpStatusCode.NotFound, null, "Bill not found"));
                }

                var billDTO = _mapper.Map<BillDTO>(bill);
                return Ok(new APIResponse(HttpStatusCode.OK, billDTO, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(HttpStatusCode.InternalServerError, null, ex.Message));
            }
        }

        [HttpGet("accounts/{accountId}/bills")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetBillsByAccountId(long accountId)
        {
            try
            {
                var bills = _billService.GetBillsByAccountId(accountId);

                if (bills == null || !bills.Any())
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "No bills found for the account"));
                }

                var billDTOs = _mapper.Map<List<BillDTO>>(bills);

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, billDTOs, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error fetching bills by account ID: " + ex.Message));
            }
        }


        [HttpGet("customers/{customerId}/bills")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetBillsByCustomerId(long customerId)
        {
            try
            {
                var bills = _billService.GetBillsByCustomerId(customerId);

                if (bills == null || !bills.Any())
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "No bills found for the customer"));
                }

                var billDTOs = _mapper.Map<List<BillDTO>>(bills);

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, billDTOs, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error fetching bills by customer ID: " + ex.Message));
            }
        }


        [HttpPost("accounts/{accountId}/bills")]
        public ActionResult<APIResponse> CreateBill(long accountId, [FromBody] BillDTO billDTO)
        {
            if (billDTO == null)
            {
                return BadRequest(new APIResponse(HttpStatusCode.BadRequest, null, "Bill data is null"));
            }

            try
            {
                var createdBill = _billService.CreateBill(accountId, _mapper.Map<Bill>(billDTO));
                return CreatedAtRoute("GetBill", new { id = createdBill.Id }, new APIResponse(HttpStatusCode.Created, _mapper.Map<BillDTO>(createdBill), "Bill created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(HttpStatusCode.InternalServerError, null, "Error creating bill: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public ActionResult<APIResponse> UpdateBill(int id, [FromBody] BillDTO billDTO)
        {
            if (billDTO == null)
            {
                return BadRequest(new APIResponse(HttpStatusCode.BadRequest, null, "Invalid bill data"));
            }

            try
            {
                var updatedBill = _billService.UpdateBill(id, _mapper.Map<Bill>(billDTO));
                if (updatedBill == null)
                {
                    return NotFound(new APIResponse(HttpStatusCode.NotFound, null, "Bill not found"));
                }

                return Ok(new APIResponse(HttpStatusCode.OK, _mapper.Map<BillDTO>(updatedBill), "Bill updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(HttpStatusCode.InternalServerError, null, "Error updating bill: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<APIResponse> DeleteBill(int id)
        {
            try
            {
                if (_billService.DeleteBill(id))
                {
                    return Ok(new APIResponse(HttpStatusCode.OK, null, "Bill deleted successfully"));
                }
                else
                {
                    return NotFound(new APIResponse(HttpStatusCode.NotFound, null, "Bill not found"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(HttpStatusCode.InternalServerError, null, "Error deleting bill: " + ex.Message));
            }
        }
    }
}
