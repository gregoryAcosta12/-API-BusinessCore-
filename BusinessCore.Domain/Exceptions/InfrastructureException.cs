using System;

namespace BusinessCore.Domain.Exceptions
{
   
    public class InfrastructureException : Exception
    {
        public InfrastructureException()
            : base("Error en la infraestructura del sistema")
        {
        }

        public InfrastructureException(string message)
            : base(message)
        {
        }

        public InfrastructureException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}