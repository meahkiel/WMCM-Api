namespace Application.SeedWorks
{
    public class AppException
    {
        public AppException(int statusCode, string message, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; private set; }
        public string Message { get; private set; }
        public string Details { get; private set; }
    }
}
