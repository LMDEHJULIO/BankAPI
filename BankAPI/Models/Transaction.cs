﻿using System.ComponentModel.DataAnnotations;
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

        [ForeignKey("AccountId")]
        public long PayeeId { get; set; }

        [Required]
        public string Medium { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Description  { get; set; }

        public Transaction()
        {
            TransactionDate = DateTime.Now;
        }

    }
}
