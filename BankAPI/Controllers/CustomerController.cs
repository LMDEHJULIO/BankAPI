using AutoMapper;
using BankAPI.Controllers;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Models.Dto.Create;
using BankAPI.Repository;
using BankAPI.Repository.IRepository;
using BankAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Controller
{


    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _db;
        private readonly IAccountRepository _accountDb;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerController> _logger;
        private readonly CustomerService _customerService;
        private readonly AccountService _accountService;


        public CustomerController(CustomerService customerService, AccountService accountService, ICustomerRepository db, IAccountRepository accountDb, IMapper mapper, ILogger<CustomerController> logger)
        {
            _db = db;
            _accountDb = accountDb;
            _mapper = mapper;
            _logger = logger;
            _customerService = customerService;
            _accountService = accountService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetCustomers()
        {
            try
            {

                IEnumerable<Customer> customers = _customerService.GetAllCustomers();
                _logger.LogInformation("Retrieved all customers");
                return new APIResponse(System.Net.HttpStatusCode.OK, customers, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error retrieving all customers" + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error fetching customers: " + ex.Message));
            }
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetCustomer(long id)
        {
            if (id == 0)
            {
                _logger.LogInformation("Error retrieving customer - ID must be greater than 0");

                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid ID"));
            }

            try
            {
          
                var customer = _customerService.GetCustomer(id);

                if (customer == null)
                {
                    _logger.LogInformation("Invalid customer ID - Customer Not Found");
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Error Fetching Account - Customer not found"));
                }
                _logger.LogInformation("Customer retrieved" + customer.ToString());
                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, customer, "Success"));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error retrieving customer" + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error retrieving customer: " + ex.Message));
            }
        }

        [HttpGet("{customerId}/accounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetCustomerAccounts(long customerId)
        {
            try
            {
            
                var customer = _customerService.GetCustomer(customerId);

                if (customer == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Error fetching customer's accounts"));
                }

                var accounts = _accountService.GetAllAccounts().Where(a => a.CustomerId == customerId);
         

                var accountDTOs = _mapper.Map<List<AccountDTO>>(accounts);

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, accountDTOs, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error fetching accounts: " + ex.Message));
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> CreateCustomer([FromBody] CustomerCreateDTO customerDTO)
        {
            if (customerDTO == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Error - Customer data is null"));
            }

            try
            {
                Customer customer = _mapper.Map<Customer>(customerDTO);
        

                _customerService.CreateCustomer(customer);

                return CreatedAtRoute("GetCustomer", new { id = customer.Id }, new APIResponse(System.Net.HttpStatusCode.Created, customer, "Customer created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error creating customer: " + ex.Message));
            }
        }

        [HttpDelete("{id}", Name = "DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<APIResponse> DeleteCustomer(long id)
        {
            if (id == 0)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid ID"));
            }

            try
            {
                var customer = _customerService.GetCustomer(id);

                if (customer == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Customer not found"));
                }
         

                _customerService.DeleteCustomer(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error deleting customer: " + ex.Message));
            }
        }

        [HttpPut("{id}", Name = "UpdateCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> UpdateCustomer(long id, [FromBody] CustomerDTO customerDTO)
        {
            if (id <= 0 || customerDTO == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid input"));
            }

            try
            {
                var editCustomer = _customerService.GetCustomer(id);

                if (editCustomer == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Customer not found"));
                }

                _mapper.Map(customerDTO, editCustomer);
        
                _customerService.UpdateCustomer(id, editCustomer);

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, editCustomer, "Customer updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error updating customer: " + ex.Message));
            }
        }
    }
}
