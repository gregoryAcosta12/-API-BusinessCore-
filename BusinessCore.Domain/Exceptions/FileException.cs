using System;

namespace BusinessCore.Domain.Exceptions
{
  
    public class FileException : Exception
    {
        public FileException()
            : base("Error al procesar el archivo")
        {
        }

        public FileException(string message)
            : base(message)
        {
        }

        public FileException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}