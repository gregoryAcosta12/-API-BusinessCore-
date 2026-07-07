using System;

namespace BusinessCore.Domain.Exceptions
{
    
    public class ConflictException : Exception
    {
        public ConflictException()
            : base("Se produjo un conflicto con el estado actual del recurso")
        {
        }

        public ConflictException(string message)
            : base(message)
        {
        }

        public ConflictException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}