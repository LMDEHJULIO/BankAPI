using BankAPI.Models;
using BankAPI.Models.Dto;

namespace BankAPI.Data
{
    public class CustomerStore
    {
        public static List<CustomerDTO> customerList = new List<CustomerDTO> {
            new CustomerDTO { Id = 1, FirstName = "Julio", LastName = "Rodriguez" }
        };
    }
}
