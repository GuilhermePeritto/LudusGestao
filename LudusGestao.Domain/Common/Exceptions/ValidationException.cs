using System;

namespace LudusGestao.Domain.Common.Exceptions
{
    public class ValidationException : DomainException
    {
        public ValidationException(string message) : base(message) { }
    }
} 