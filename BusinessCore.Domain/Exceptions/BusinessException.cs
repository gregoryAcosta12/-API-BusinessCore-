using System;

namespace BusinessCore.Domain.Exceptions
{
    
    public class BusinessException : Exception
    {
        public string BusinessCode { get; }

        public BusinessException(string message)
            : base(message)
        {
        }

        public BusinessException(string message, string businessCode)
            : base(message)
        {
            BusinessCode = businessCode;
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}