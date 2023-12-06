using AutoMapper;
using bankapi.models;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Models.Dto.Create;
using BankAPI.Repository.IRepository;
using BankAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BankAPI.Controllers
{
    [Route("api/withdrawals")]
    [ApiController]
    public class WithdrawalController : ControllerBase

    {
        private readonly IWithdrawalRepository _db;
        private readonly IAccountRepository _accountDb;
        private readonly IMapper _mapper;
        private readonly WithdrawalService _withdrawalService;

        public WithdrawalController(WithdrawalService withdrawalService, IWithdrawalRepository db, IAccountRepository accountDb, IMapper mapper)
        {
            _db = db;
            _accountDb = accountDb;
            _mapper = mapper;
            _withdrawalService = withdrawalService;
        }

        [HttpGet]
        public ActionResult<APIResponse> GetWithdrawals()
        {
            try
            {
     
                var withdrawals = _withdrawalService.GetWithdrawals();
                var withdrawalDTOs = _mapper.Map<List<WithdrawalDTO>>(withdrawals);
                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, withdrawalDTOs, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, ex.Message));
            }
        }

        [HttpGet("{id}", Name = "GetWithdrawal")]
        public ActionResult<APIResponse> GetWithdrawal(long id)
        {
            if (id == 0)
            {
                return BadRequest(new APIResponse(HttpStatusCode.BadRequest, null, "Invalid ID"));
            }

            try
            {
             
                var withdrawal = _withdrawalService.GetWithdrawal(id);

                if (withdrawal == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Withdrawal not found"));
                }

                var withdrawalDTO = _mapper.Map<WithdrawalDTO>(withdrawal);

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, withdrawalDTO, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, ex.Message));
            }
        }

        [HttpGet("{accountId}/withdrawals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetAccountWithdrawals(long accountId)
        {
            try
            {
                var account = _accountDb.Get(accountId);

                if (account == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Account not found"));
                }

                var withdrawals = _withdrawalService.GetAccountWithdrawals(accountId);

                if (withdrawals == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "No withdrawals found"));
                }

                var withdrawalDTOs = _mapper.Map<List<WithdrawalDTO>>(withdrawals);

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, withdrawalDTOs, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error fetching account/deposits: " + ex.Message));
            }
        }

        [HttpPost("accounts/{accountId}/withdrawals")]
        public ActionResult<APIResponse> CreateWithdrawal([FromBody] WithdrawalCreateDTO withdrawalDTO, long accountId)
        {
            if (withdrawalDTO == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Withdrawal data is null"));
            }

            try
            {
                var withdrawal = _mapper.Map<Withdrawal>(withdrawalDTO);
                var account = _accountDb.Get(accountId);

                if (account == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Account not found"));
                }

                withdrawal.AccountId = accountId;

                _withdrawalService.CreateWithdrawal(withdrawal, accountId);

                return CreatedAtRoute("GetWithdrawal", new { id = withdrawal.Id }, new APIResponse(System.Net.HttpStatusCode.Created, _mapper.Map<WithdrawalDTO>(withdrawal), "Withdrawal created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error creating withdrawal: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public ActionResult<APIResponse> UpdateWithdrawal(long id, [FromBody] WithdrawalDTO withdrawalDTO)
        {
            if (withdrawalDTO == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, "Invalid withdrawal data"));
            }

            try
            {
                var updatedWithdrawal = _withdrawalService.UpdateWithdrawal(id, _mapper.Map<Withdrawal>(withdrawalDTO));

                if (updatedWithdrawal == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, "Withdrawal not found"));
                }

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, _mapper.Map<WithdrawalDTO>(updatedWithdrawal), "Withdrawal updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error updating withdrawal: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<APIResponse> DeleteWithdrawal(long id)
        {
            try
            {
                var withdrawal = _db.Get(id);

                if (withdrawal == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Withdrawal not found"));
                }

                _withdrawalService.DeleteWithdrawal(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error deleting withdrawal: " + ex.Message));
            }
        }
    }
}
