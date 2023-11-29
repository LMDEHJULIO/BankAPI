using System.Net;

namespace BankAPI.Models
{


    public class APIResponse
    {
     
        public HttpStatusCode Code { get; set; }

        public int StatusCode => (int)Code;

        public string? Message { get; set; }
        //public List<string> ErrorMessages { get; set; }

        public object Data { get; set;  }

        public APIResponse(HttpStatusCode statusCode, object result, string mssg = null) {
            Code = statusCode;
            Data = result;
            Message = mssg;
        }

        public APIResponse() { }
    }
}
