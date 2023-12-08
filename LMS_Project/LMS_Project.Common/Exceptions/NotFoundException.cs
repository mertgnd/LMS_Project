using System;
using System.Collections.Generic;
using System.Text;

namespace LMS_Project.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public override string Message { get; }

        public NotFoundException(string message) 
        {
            Message = message;
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
