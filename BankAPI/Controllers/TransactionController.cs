﻿using AutoMapper;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase

    {

        private readonly ITransactionRepository _db;
        private readonly IAccountRepository _accountDb;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionRepository db, IAccountRepository accountDb, IMapper mapper)
        {
            _db = db;
            _accountDb = accountDb;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<APIResponse> GetTransaction()
        {
            try
            {
                var transactions = _db.GetAll();
                var transactionDTOs = _mapper.Map<List<TransactionDTO>>(transactions);
                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, transactionDTOs, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, "Error fetching transactions: " + ex.Message));
            }
        }

    }
}
