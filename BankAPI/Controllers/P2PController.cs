using AutoMapper;
using BankAPI.Models.Dto;
using BankAPI.Models;
using BankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using BankAPI.Services;
using BankAPI.Models.Dto.Update;

namespace BankAPI.Controllers
{

    [Route("api/p2p")]
    [ApiController]
    public class P2PController : ControllerBase
    {
        private readonly IP2PRepository _db;
        private readonly IAccountRepository _accountDb;
        private readonly IMapper _mapper;
        private readonly P2PService _p2pService;
        private readonly AccountService _accountService;
        private readonly IMapper _mapper2;

        public P2PController(P2PService p2pService, AccountService accountService, IP2PRepository db, IAccountRepository accountDb, IMapper mapper)
        {
            _db = db;
            _accountDb = accountDb;
            _mapper = mapper;
            _p2pService = p2pService;
            _accountService = accountService;
            _accountService = accountService;
        }

        [HttpGet]
        public ActionResult<APIResponse> GetP2P()
        {
            try
            {
            
                var p2pTransactions = _p2pService.GetAllP2P();
                var p2pDTOs = _mapper.Map<List<P2PDTO>>(p2pTransactions);
                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, p2pDTOs, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, ex.Message));
            }
        }

        [HttpPost("accounts/{accountId}/p2p")]
        public ActionResult<APIResponse> CreateP2P([FromBody] P2PDTO p2pDTO, long accountId)
        {
            if (p2pDTO == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "P2P data is null"));
            }

            try
            {
                var p2p = _mapper.Map<P2P>(p2pDTO);
                
                var account = _accountService.GetAccountById(accountId);

                var recipientAccount = _accountService.GetAccountById(p2pDTO.RecipientID);

                if (account == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Sending Account not found"));
                }

                if (recipientAccount == null) {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Recipient Account not found"));
                }

                p2p.AccountId = accountId;

                _p2pService.CreateP2P(p2p, accountId, p2pDTO.RecipientID);

                return CreatedAtAction(nameof(GetP2PById), new { id = p2p.Id }, new APIResponse(System.Net.HttpStatusCode.Created, _mapper.Map<P2PDTO>(p2p), "P2P transaction created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error creating P2P transaction: " + ex.Message));
            }
        }

        [HttpGet("{id}", Name = "GetP2PById")]
        public ActionResult<APIResponse> GetP2PById(long id)
        {
            try
            {
                var p2p = _p2pService.GetP2PById(id);

                if (p2p == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "P2P transaction not found"));
                }

                var p2pDTO = _mapper.Map<P2PDTO>(p2p);
                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, p2pDTO, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public ActionResult<APIResponse> UpdateP2P(long id, [FromBody] P2PUpdateDTO p2pDTO)
        {
            if (p2pDTO == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid P2P data"));
            }

            try
            {
                var updateP2P = _mapper.Map<P2P>(p2pDTO);

                var p2p = _p2pService.GetP2PById(id);


                if (p2p == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "P2P transaction not found"));
                }

                _mapper.Map(p2pDTO, p2p);
                _mapper.Map(p2pDTO, p2p);

                _p2pService.UpdateP2P(id, p2p);

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, _mapper.Map<P2PDTO>(p2p), "P2P transaction updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error updating P2P transaction: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<APIResponse> DeleteP2P(long id)
        {
            try
            {
                var p2p = _p2pService.GetP2PById(id);

                if (p2p == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "P2P transaction not found"));
                }

                _p2pService.DeleteP2P(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error deleting P2P transaction: " + ex.Message));
            }
        }


    }
}
