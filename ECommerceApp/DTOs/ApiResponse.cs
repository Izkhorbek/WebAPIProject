using System.Net;

namespace ECommerceApp.DTOs
{
    public class ApiResponse<T>
    {
        //HTTP Status Code of the response
        public int  StatusCode { get; set; }

        //Indicates whether the request was successful
        public bool Success { get; set; }

        //Response data in case of success
        public T Data { get; set; }

        //List of errors in case of failure
        public List<string> Errors { get; set; }

        public ApiResponse() 
        {
            Success = true;
            Errors = new List<string>();
        }

        public ApiResponse(int statusCode, T data)
        {
            StatusCode = statusCode;
            Success = true;
            Data = data;
            Errors = new List<string>();
        }
        public ApiResponse(HttpStatusCode statusCode, T data)
        {
            StatusCode = (int)statusCode;
            Success = true;
            Data = data;
            Errors = new List<string>();
        }

        public ApiResponse(int statusCode, List<string> errors)
        {
            StatusCode = statusCode;
            Success = false;
            Errors = errors;
        }

        public ApiResponse(int statusCode, string error)
        {
            StatusCode = statusCode;
            Success = false;
            Errors = new List<string> { error };
        }

        public ApiResponse(HttpStatusCode statusCode, string error)
        {
            StatusCode = (int)statusCode;
            Success = false;
            Errors = new List<string> { error };
        }
    }
}
