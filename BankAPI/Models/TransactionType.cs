using System.Runtime.Serialization;

namespace BankAPI.Models
{
    public enum TransactionType
    {
        [EnumMember(Value = "P2P")]
        P2P,
        [EnumMember(Value = "Deposit")]
        Deposit,
        [EnumMember(Value = "Withdrawal")]
        Withdrawal
    }
}
