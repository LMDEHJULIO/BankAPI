using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id  { get; set; }

        [Required]
        public String type { get; set; }

        
        public String transactionDate { get; set; }


        public String status { get; set; }

        public long payeeId { get; set; }

        public String medium { get; set; }

        public Double amount { get; set; }

        public String description  { get; set; }

    }
}
