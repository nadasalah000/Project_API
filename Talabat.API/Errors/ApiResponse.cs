
namespace Talabat.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int _statuscode,string? _message = null)
        {
          StatusCode = _statuscode;
            Message = _message?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "You Are Not Authourize",
                404 => "Resourse Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
