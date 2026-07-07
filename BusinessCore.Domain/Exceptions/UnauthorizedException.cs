using System;

namespace BusinessCore.Domain.Exceptions
{
   
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
            : base("No tiene autorización para realizar esta acción")
        {
        }

        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}