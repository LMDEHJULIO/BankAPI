﻿namespace BankAPI.Models.Dto
{
    public class AccountDTO
    {
        public long Id { get; set; }
        public Type Type { get; set; }

        public string Nickname { get; set; }

        public int Rewards { get; set; }

        public double Balance { get; set; }

        //public long CustomerId { get; set; }

        public Customer Customer { get; set; }
    }
}
