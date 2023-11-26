namespace BankAPI.Models.Dto
{
    public class AccountDTO
    {
        public long Id { get; set; }
        public Type Type { get; set; }

        public string Nickname { get; set; }

        public int Rewards { get; set; }

        public double Balance { get; set; }

        public int CustomerId { get; set; }
    }
}
