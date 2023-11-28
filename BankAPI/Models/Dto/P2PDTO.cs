using Org.BouncyCastle.Cms;

namespace BankAPI.Models.Dto
{
    public class P2PDTO : TransactionDTO
    {
        public long RecipientID {get; set; }
    }
}
