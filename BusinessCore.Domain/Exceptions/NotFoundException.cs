using System;

namespace BusinessCore.Domain.Exceptions
{
    
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base("El recurso solicitado no fue encontrado")
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string entityName, int id)
            : base($"El {entityName} con ID {id} no fue encontrado")
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}