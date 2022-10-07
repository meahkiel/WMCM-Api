using Microsoft.AspNetCore.Http;


namespace Infrastructure.Core
{
    public class UploadCsvFileRequest
    {
        public IFormFile File { get; set; }
    }

    public class UploadCsvFileResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public T Value { get; set; }
    }
}
