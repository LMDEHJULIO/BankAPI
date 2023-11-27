using System.Numerics;

namespace BankAPI.Models.Dto
{
    public class BillDTO
    {
      
            public int Id { get; set; }

            public Status Status { get; set; }

            public string Payee { get; set; }

            public string Nickname { get; set; }

            public string CreationDate { get; set; }

            public string PaymentDate { get; set; }

            public int RecurringDate { get; set; }

            public string UpcomingPaymentDate { get; set; }

            public double PaymentAmount { get; set; }

            public long AccountId { get; set; }
        }
    
}
