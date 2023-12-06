using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Models.Dto
{
    public class CustomerDTO
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; } 

        public ICollection<AddressDTO> Address { get; set; } 

        public DateTime CreatedDate { get; set; }
    }
}
