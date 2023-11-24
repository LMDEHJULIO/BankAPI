using System.Runtime.Serialization;

namespace BankAPI.Models
{
    public enum Type
    {
        [EnumMember(Value = "Savings")]
        Savings,

        [EnumMember(Value = "Checking")]
        Checking,

        [EnumMember(Value = "Credit")]
        Credit,
    }
}
