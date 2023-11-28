using AutoMapper;
using BankAPI.Models.Dto;
using BankAPI.Models;
using BankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BankAPI.Controllers
{

    [Route("api/p2p")]
    [ApiController]
    public class P2PController : ControllerBase
    {

        private readonly IP2PRepository _db;
        private readonly IAccountRepository _accountDb;

        private readonly IMapper _mapper;

        public P2PController(IP2PRepository db, IAccountRepository accountDb, IMapper mapper)
        {
            _db = db;
            _accountDb = accountDb;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<P2P>> GetP2P()
        {
            var p2p = _db.GetAll().Where(d => d.Type == TransactionType.P2P);

            //return Ok(_mapper.Map<List<BillDTO>>(bills));

            return Ok(_mapper.Map<List<P2PDTO>>(p2p));
        }

        [HttpPost("accounts/{accountId}/p2p")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<P2P> CreateP2P([FromBody] P2PDTO P2PDTO, long accountId)
        {

            var p2p = _mapper.Map<P2P>(P2PDTO);

            if (p2p == null)
            {

                return BadRequest();
            }

            if (p2p.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var account = _accountDb.Get(accountId);
            var recipientAccount = _accountDb.Get(p2p.RecipientId);

            account.Balance -= p2p.Amount;
            recipientAccount.Balance += p2p.Amount;

            if (account == null)
            {
                return NotFound("Account not Found");
            }

            p2p.AccountId = accountId;

            _db.Create(p2p);

            _db.Save();
            _accountDb.Save();

            return CreatedAtRoute("GetDeposit", new { id = p2p.Id }, p2p);
        }
    }
}
