using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace BankAPI.Models
{
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       
        public Status Status { get; set; }

        [Required]
        public string Payee { get; set; }

        [Required]
        public string Nickname { get; set; }

        [Required]
        public string CreationDate { get; set; }

        [Required]
        public string PaymentDate { get; set; }

        [Required]
        public int RecurringDate { get; set; }

        [Required]
        public string UpcomingPaymentDate { get; set; }


        [Required]
        public double PaymentAmount { get; set; }

      
        [ForeignKey("AccountId")]
        public long AccountId { get; set; }

        public Account Account { get; set; }
    }
}
