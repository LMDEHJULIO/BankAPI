using BankAPI.Models.Dto.Update;

namespace BankAPI.Models.Dto.Create
{
    public class P2PCreateDTO : TransactionCreateDTO
    {
        public long RecipientID { get; set; }
    }
}
