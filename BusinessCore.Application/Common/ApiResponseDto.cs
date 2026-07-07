using System.Collections.Generic;

namespace BusinessCore.Application.DTOs.Common
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }

        public ApiResponseDto()
        {
            Errors = new List<string>();
        }

        public ApiResponseDto(T data, string message = null)
        {
            Success = true;
            Data = data;
            Message = message ?? "Operación exitosa";
            StatusCode = 200;
            Errors = new List<string>();
        }

        public ApiResponseDto(string message, int statusCode = 400, List<string> errors = null)
        {
            Success = false;
            Message = message;
            StatusCode = statusCode;
            Errors = errors ?? new List<string>();
        }
    }
}