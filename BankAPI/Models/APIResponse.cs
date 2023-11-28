using System.Net;

namespace BankAPI.Models
{


    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }

        public object Result { get; set;  }

        public APIResponse(HttpStatusCode statusCode, bool success, object result, List<string> errorMessages = null) { 
            StatusCode = statusCode;
            IsSuccess = success;
            ErrorMessages = errorMessages;
            Result = result;
        }

        public APIResponse() { }
    }
}
