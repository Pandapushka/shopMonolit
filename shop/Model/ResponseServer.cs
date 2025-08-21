using System.Net;

namespace shop.Model
{
    public class ResponseServer<T>
    {
       
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; } 
        public T? Result { get; set; }

        public static ResponseServer<T> Success(T data, int statusCode = 200)
        {
            return new ResponseServer<T>
            {
                StatusCode = statusCode,
                IsSuccess = true,
                Result = data
            };
        }
        public static ResponseServer<T> Error(string message, int statusCode = 400)
        {
            return new ResponseServer<T>
            {
                StatusCode = statusCode,
                IsSuccess = false,
                ErrorMessage = message
            };
        }
    }
}
