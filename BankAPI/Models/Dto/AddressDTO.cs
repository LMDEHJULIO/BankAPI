using Microsoft.Identity.Client;

namespace BankAPI.Models.Dto
{
    public class AddressDTO
    {
        public int Id { get; set; }
        
        public string StreetNumber { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }
    }
}
