using BankAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace bankapi.models
{
    public class Withdrawal : Transaction
    {
        [ForeignKey("AccountId")]
        public long PayerId {get; set;}
    }
}
