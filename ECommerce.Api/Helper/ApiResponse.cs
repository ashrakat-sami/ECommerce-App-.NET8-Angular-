namespace ECommerce.Api.Helper
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message=null)
        {
            StatusCode = statusCode;
            Message =message ?? GetRsponseMessage(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetRsponseMessage(int statusCode)
        {
            return statusCode switch
            {
                200 => "Done",
                400 => "The server could not understand the request due to invalid syntax.",
                401 => "The client must authenticate itself to get the requested response.",
                404 => "Not found resources.",
                500 => "The server has encountered a situation it doesn't know how to handle.",
                _ => "null",
            };
        }
    }
}
