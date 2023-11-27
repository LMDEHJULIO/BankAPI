namespace BankAPI.Models.Dto
{
    public class DepositDTO : TransactionDTO
    {
        public long PayeeId { get; set; }
    }
}
