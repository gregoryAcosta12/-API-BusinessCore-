using System;

namespace BusinessCore.Domain.Exceptions
{
   
    public class DatabaseException : Exception
    {
        public DatabaseException()
            : base("Error al acceder a la base de datos")
        {
        }

        public DatabaseException(string message)
            : base(message)
        {
        }

        public DatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}