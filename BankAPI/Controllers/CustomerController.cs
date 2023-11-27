using AutoMapper;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
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

        protected APIResponse _response; 

        private readonly ICustomerRepository _db;

        private readonly IMapper _mapper; 

        public CustomerController(ICustomerRepository db, IMapper mapper) {
            _db = db; 
            _mapper = mapper;
            this._response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<APIResponse> GetCustomers() {

            try
            {
                IEnumerable<Customer> customers = _db.GetAll(c => c.Address);

                _response.Result = _mapper.Map<List<CustomerDTO>>(customers);
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;

                return Ok(_response);
            } catch (Exception ex) {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;

        }

        [HttpGet("{id}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDTO> GetCustomer(int id) {
            if (id == 0) {
                return BadRequest();
            }
            var customer = _db.Get(id, c => c.Address);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDTO> CreateCustomer([FromBody] Customer customer) {
            if (customer == null)
            {

                return BadRequest();
            }

            if (customer.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            _db.Create(customer);

            _db.Save();

            return CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteCustomer")]
        public IActionResult DeleteCustomer(int id) {
            if (id == 0)
            {
                return BadRequest();
            }

            var customer = _db.Get(id);

            if (customer == null)
            {
                return NotFound();
            }

            _db.Remove(customer);

            _db.Save();

            return NoContent();
        }

        [HttpPut("{id:int}", Name="UpdateCustomer")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer customer) {
            if (id == null)
            {
                return BadRequest();
            }

            var editCustomer = _db.Get(id);

            editCustomer.FirstName = customer.FirstName;
            editCustomer.LastName = customer.LastName;
            //editCustomer.Address = customer.Address;

            _db.Update(editCustomer);

            _db.Save();

            return NoContent();
        }



    }
}
