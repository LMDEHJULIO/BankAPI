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
        public ActionResult<APIResponse> GetWithdrawals()
        {
            try
            {
                var withdrawals = _db.GetAll();
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
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid ID"));
            }

            try
            {
                var withdrawal = _db.Get(id);
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

                var withdrawals = _db.GetAll().Where(d => d.AccountId == accountId);

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
        public ActionResult<APIResponse> CreateWithdrawal([FromBody] WithdrawalDTO withdrawalDTO, long accountId)
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

                account.Balance -= withdrawal.Amount; // Ensure business logic is correct for balance update

                _db.Create(withdrawal);
                _db.Save();
                _accountDb.Save();

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
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid withdrawal data"));
            }

            try
            {
                var withdrawal = _db.Get(id);
                if (withdrawal == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Withdrawal not found"));
                }

                _mapper.Map(withdrawalDTO, withdrawal);
                _db.Update(withdrawal);
                _db.Save();

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, _mapper.Map<WithdrawalDTO>(withdrawal), "Withdrawal updated successfully"));
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

                _db.Remove(withdrawal);
                _db.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error deleting withdrawal: " + ex.Message));
            }
        }
    }
}
