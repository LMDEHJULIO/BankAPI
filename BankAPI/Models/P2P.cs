using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models
{
    public class P2P : Transaction
    {
        [ForeignKey("RecipientAccount")]
        public long? RecipientId { get; set; }

        public virtual Account RecipientAccount { get; set; }
    }
}
