using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
    }
}
