namespace BankAPI.Models.Dto
{
    public class TransactionDTO
    {
        public long Id { get; set; }

        public TransactionType Type { get; set; }

        public DateTime TransactionDate { get; set; }

        public TransactionStatus Status { get; set; }

        public Medium? Medium { get; set; }

        public double Amount { get; set; }

        public long AccountId { get; set; }

        public string Description { get; set; }
    }
}
