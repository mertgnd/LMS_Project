using System;

namespace LMS_Project.Common.Exceptions
{
    public class AppException : Exception
    {
        public override string Message { get; }

        public AppException(string message) 
        {
            Message = message;
        }

        public AppException(string message, Exception innerException) : base(message, innerException) { }
    }
}