namespace YumaIdentity.Application.Common.Exceptions
{
    using System;
    using System.Collections.Generic;

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(IEnumerable<string> errors) : base("One or more validation failures have occurred.")
        {
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; } = [];
    }
}
