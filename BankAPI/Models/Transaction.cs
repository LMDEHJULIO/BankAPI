using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id  { get; set; }

        [Required]
        public TransactionType Type { get; set; }
         
        public DateTime TransactionDate { get; set; }

        [Required]
        public TransactionStatus Status { get; set; }

        [Required]
        public Medium? Medium { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Description  { get; set; }

        [ForeignKey("Account")]
        public long AccountId { get; set; }

        public virtual Account Account { get; set; }


        public Transaction()
        {
            TransactionDate = DateTime.Now;
        }

    }
}
