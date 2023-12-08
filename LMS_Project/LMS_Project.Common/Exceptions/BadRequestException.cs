using System;

namespace LMS_Project.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public override string Message { get; }

        public BadRequestException(string message)
        {
            Message = message;
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
}