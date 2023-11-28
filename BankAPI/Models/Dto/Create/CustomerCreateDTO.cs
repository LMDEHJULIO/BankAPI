namespace BankAPI.Models.Dto.Create
{
    public class CustomerCreateDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<AddressDTO> Address { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
