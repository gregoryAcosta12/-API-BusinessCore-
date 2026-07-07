using System.Collections.Generic;

namespace BusinessCore.Application.DTOs.Common
{
    public class ErrorResponseDto
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
        public List<ValidationErrorDto> ValidationErrors { get; set; }
        public string RequestId { get; set; }
    }

    public class ValidationErrorDto
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public object AttemptedValue { get; set; }
    }
}