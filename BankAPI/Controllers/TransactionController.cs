//using BankAPI.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Net.NetworkInformation;
//using System.Transactions;
//using Transaction = BankAPI.Models.Transaction;

//namespace BankAPI.Controllers
//{
//    [Route("/api/transactions")]
//    [ApiController]
//    public class TransactionController
//    {
//        [HttpGet]
//        public IEnumerable<Transaction> GetTransactions() {
//            return new List<Transaction> {
//                new Transaction(
//                1, // Replace with the desired values
//                "Credit",
//                "2023-11-21",
//                "Completed",
//                12345,
//                "Online",
//                100.50, // Replace with the desired amount
//                "Payment for goods"
//                    )
//           };
//        }
//    }
//}
