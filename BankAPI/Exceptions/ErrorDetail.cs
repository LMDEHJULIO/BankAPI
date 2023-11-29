using MySqlX.XDevAPI.Common;

namespace BankAPI.Exceptions
{
    public class ErrorDetail
    {
        public long TimeStamp { get; set; }
        public int Status { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public string DeveloperMessage { get; set; }

        public Dictionary<string, List<ValidationError>> Errors { get; set; }
    }


}
