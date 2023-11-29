using AutoMapper;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using BankAPI.Models.Dto.Create;
using BankAPI.Repository;
using BankAPI.Repository.IRepository;
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
        private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetCustomers()
        {
            try
            {
                IEnumerable<Customer> customers = _db.GetAll(c => c.Address);
                return new APIResponse(System.Net.HttpStatusCode.OK, customers, "Success");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error fetching customers: " + ex.Message));
            }
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<APIResponse> GetCustomer(int id)
        {
            if (id == 0)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid ID"));
            }

            try
            {
                var customer = _db.Get(id, c => c.Address);
                if (customer == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Customer not found"));
                }

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, customer, "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error retrieving customer: " + ex.Message));
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
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Customer data is null"));
            }

            try
            {
                Customer customer = _mapper.Map<Customer>(customerDTO);
                _db.Create(customer);
                _db.Save();

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

        public ActionResult<APIResponse> DeleteCustomer(int id)
        {
            if (id == 0)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid ID"));
            }

            try
            {
                var customer = _db.Get(id);
                if (customer == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Customer not found"));
                }

                _db.Remove(customer);
                _db.Save();

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
        public ActionResult<APIResponse> UpdateCustomer(int id, [FromBody] CustomerDTO customerDTO)
        {
            if (id <= 0 || customerDTO == null)
            {
                return BadRequest(new APIResponse(System.Net.HttpStatusCode.BadRequest, null, "Invalid input"));
            }

            try
            {
                var editCustomer = _db.Get(id);
                if (editCustomer == null)
                {
                    return NotFound(new APIResponse(System.Net.HttpStatusCode.NotFound, null, "Customer not found"));
                }

                _mapper.Map(customerDTO, editCustomer);
                _db.Update(editCustomer);
                _db.Save();

                return Ok(new APIResponse(System.Net.HttpStatusCode.OK, editCustomer, "Customer updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse(System.Net.HttpStatusCode.InternalServerError, null, "Error updating customer: " + ex.Message));
            }
        }
    }
}
