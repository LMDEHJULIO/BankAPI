namespace BankAPI.Models.Dto.Create
{
    public class AccountCreateDTO
    {
        public long Id { get; set; }
        public Type Type { get; set; }

        public string Nickname { get; set; }

        public int Rewards { get; set; }

        public double Balance { get; set; }

        public long CustomerId { get; set; }
    }
}
