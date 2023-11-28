using System.Runtime.Serialization;

namespace BankAPI.Models
{
    public enum Medium
    {
        [EnumMember(Value = "Rewards")]
        Rewards,
        [EnumMember(Value = "Balance")]
        Balance
    }
}
