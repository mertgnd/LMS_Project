using System;

namespace LMS_Project.Common.Exceptions
{
    public class ConflictException : Exception
    {
        public override string Message { get; }

        public ConflictException(string message) 
        {
            Message = message;
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException) { }
    }
}