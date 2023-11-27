namespace BankAPI.Models.Dto
{
    public class WithdrawalDTO : TransactionDTO
    {
        public long PayerId { get; set; }
    }
}
