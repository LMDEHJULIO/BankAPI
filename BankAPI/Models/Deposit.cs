using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace BankAPI.Models
{
    public class Deposit : Transaction
    {
        [ForeignKey("AccountId")]
        public long PayeeId { get; set; }
    }
}
