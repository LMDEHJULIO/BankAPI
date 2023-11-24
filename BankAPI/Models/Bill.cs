using System.Numerics;

namespace BankAPI.Models
{
    public class Bill
    {
        public int id { get; set; }

        public string status { get; set; }

        public string payee { get; set; }

        public string nickname { get; set; }

        public string creationDate { get; set; }

        public string paymentDate { get; set; }

        public BigInteger recurringDate { get; set; }

        public string upcomingPaymentDate { get; set; }

        public double paymentAmount { get; set; }

        public string acccountId { get; set; }

        public Bill(int id, string status, string payee, string nickname, string creationDate, string paymentDate, BigInteger recurringDate, string upcomingPaymentDate, double paymentAmount, string acccountId)
        {
            this.id = id;
            this.status = status;
            this.payee = payee;
            this.nickname = nickname;
            this.creationDate = creationDate;
            this.paymentDate = paymentDate;
            this.recurringDate = recurringDate;
            this.upcomingPaymentDate = upcomingPaymentDate;
            this.paymentAmount = paymentAmount;
            this.acccountId = acccountId;
        }
    }
}
