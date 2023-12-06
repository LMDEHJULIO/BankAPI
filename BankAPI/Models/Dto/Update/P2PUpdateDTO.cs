using BankAPI.Models.Dto.Create;

namespace BankAPI.Models.Dto.Update
{
    public class P2PUpdateDTO : TransactionUpdateDTO
    {
        public long RecipientID { get; set; }
    }
}
