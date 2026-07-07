using System;

namespace BusinessCore.Domain.Exceptions
{
  
    public class BadRequestException : Exception
    {
        public BadRequestException()
            : base("La solicitud es inválida")
        {
        }

        public BadRequestException(string message)
            : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}