using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankAPI.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public Type Type { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
        public int Rewards { get; set; }


        [Required]
        public double Balance { get; set; }

     
        public int CustomerId { get; set; }

        [JsonIgnore]
        public virtual ICollection<Transaction> Transactions { get; set; }
        
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        public Account() { 
            Transactions = new HashSet<Transaction>();
        }
    }
}
