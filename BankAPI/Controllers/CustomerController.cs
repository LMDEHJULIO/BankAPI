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

                //_response.Result = _mapper.Map<List<CustomerDTO>>(customers);
                //_response.IsSuccess = true;
                //_response.StatusCode = System.Net.HttpStatusCode.OK;

                //return Ok(_response);

                return new APIResponse(System.Net.HttpStatusCode.OK, true, customers);
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
        public ActionResult<APIResponse> GetCustomer(int id) {
            if (id == 0) {
                return BadRequest();
            }
            var customer = _db.Get(id, c => c.Address);

            if (customer == null)
            {
                return NotFound();
            }

            _response.Result = _mapper.Map<CustomerDTO>(customer);
            _response.IsSuccess = true;
            _response.StatusCode = System.Net.HttpStatusCode.OK;

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<APIResponse> CreateCustomer([FromBody] CustomerDTO customerDTO) {

            try
            {
                if (customerDTO == null)
                {

                    return BadRequest();
                }

                if (customerDTO.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                Customer customer = _mapper.Map<Customer>(customerDTO);

                _db.Create(customer);
                _db.Save();

                _response.Result = _mapper.Map<CustomerDTO>(customer);
                _response.StatusCode = System.Net.HttpStatusCode.Created;



                return CreatedAtRoute("GetCustomer", new { id = customer.Id }, _response);
            } catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

                return _response;
            }
            return _response; 
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteCustomer")]
        public ActionResult<APIResponse> DeleteCustomer(int id) {


            try
            {
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

                _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return _response;

            }
            catch (Exception ex) {
                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("{id:int}", Name="UpdateCustomer")]
        public ActionResult<APIResponse> UpdateCustomer(int id, [FromBody] Customer customerDTO) {
            try {
                if (id == null)
                {
                    return BadRequest();
                }

                var editCustomer = _db.Get(id);

                editCustomer.FirstName = customerDTO.FirstName;
                editCustomer.LastName = customerDTO.LastName;
                //editCustomer.Address = customer.Address;

                _db.Update(editCustomer);

                _db.Save();

                _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            } catch (Exception ex)
            {

                _response.IsSuccess = false;

                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;

        }



    }
}
