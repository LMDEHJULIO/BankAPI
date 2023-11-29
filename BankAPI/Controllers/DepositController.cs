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
    public class DepositController : ControllerBase {

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetDeposits()
        {
            try
            {
                var deposits = _db.GetAll().Where(d => d.Type == TransactionType.Deposit);
                var depositDTOs = _mapper.Map<List<DepositDTO>>(deposits);
                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, depositDTOs, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, ex.Message));
            }
        }

        [HttpGet("{id}", Name = "GetDeposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetDeposit(long id)
        {
            if (id == 0)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid ID - must be > 0"));
            }

            try
            {
                var deposit = _db.Get(id);

                if (deposit == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Error fetching deposit with id."));
                }

                var depositDTO = _mapper.Map<DepositDTO>(deposit);

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, depositDTO, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, ex.Message));
            }
        }

        [HttpGet("{accountId}/deposits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetAccountDeposits(long accountId)
        {
            try
            {
                var account = _accountDb.Get(accountId);

                if (account == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Account not found"));
                }

                var deposits = _db.GetAll().Where(d => d.AccountId == accountId);
        
                if (deposits == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Account not found"));
                }

                var depositDTOs = _mapper.Map<List<DepositDTO>>(deposits);

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, depositDTOs, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error fetching account/deposits: " + ex.Message));
            }
        }

        [HttpPost("accounts/{accountId}/deposits")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<APIResponse> CreateDeposit([FromBody] DepositDTO depositDTO, long accountId)
        {
            if (depositDTO == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Bad Request: Deposit Data Invalid"));
            }

            try
            {
                var deposit = _mapper.Map<Deposit>(depositDTO);
                var account = _accountDb.Get(accountId);
                if (account == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Error creating deposit: Account not found"));
                }

                deposit.AccountId = accountId;

                account.Balance += deposit.Amount; 

                _db.Create(deposit);
                _db.Save();
                _accountDb.Save();

                return CreatedAtRoute("GetDeposit", new { id = deposit.Id }, new APIResponse(System.Net.HttpStatusCode.Created, _mapper.Map<DepositDTO>(deposit), "Deposit created successfully and Added to Account"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error creating deposit: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> UpdateDeposit(long id, [FromBody] DepositDTO depositDTO)
        {
            if (depositDTO == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid deposit data"));
            }

            if (depositDTO.Status == TransactionStatus.Completed) {

                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Deposit already complete - cannot be altered."));
            }

            try
            {
                var deposit = _db.Get(id);
                if (deposit == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Deposit ID does not exist"));
                }

                _mapper.Map(depositDTO, deposit);
                _db.Update(deposit);
                _db.Save();

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, _mapper.Map<DepositDTO>(deposit), "Accepted deposit modification"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error updating deposit: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> DeleteDeposit(long id)
        {
            try
            {
                var deposit = _db.Get(id);
                if (deposit == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "This id does not exist in deposits"));
                }

                _db.Remove(deposit);
                _db.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error deleting deposit: " + ex.Message));
            }
        }
    }
}
