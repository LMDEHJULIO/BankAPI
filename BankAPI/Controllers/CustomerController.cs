using AutoMapper;
using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Models.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Controller
{


    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper; 

        public CustomerController(ApplicationDbContext db, IMapper mapper) {
            _db = db; 
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CustomerDTO>> GetCustomers() {

            IEnumerable<Customer> customers = _db.Customers
            .Include(c => c.Address) // Include the Addresses collection
            .ToList();


            return Ok(_mapper.Map<List<CustomerDTO>>(customers));

        }

        [HttpGet("{id}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerDTO> GetCustomer(int id) {
            if (id == 0) {
                return BadRequest();
            }
            var customer = _db.Customers.Include(c => c.Address).ToList().FirstOrDefault(u => u.Id == id);

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

            _db.Customers.Add(customer);

            _db.SaveChanges();

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

            var customer = _db.Customers.FirstOrDefault(u => u.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            _db.Customers.Remove(customer);

            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}", Name="UpdateCustomer")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer customer) {
            if (id == null)
            {
                return BadRequest();
            }

            var editCustomer = _db.Customers.FirstOrDefault(u => u.Id == id);

            editCustomer.FirstName = customer.FirstName;
            editCustomer.LastName = customer.LastName;
            //editCustomer.Address = customer.Address;

            _db.Customers.Update(editCustomer);

            _db.SaveChanges();

            return NoContent();
        }



    }
}
