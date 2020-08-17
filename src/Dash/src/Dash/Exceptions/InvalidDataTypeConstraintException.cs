using System;

namespace Dash.Exceptions
{
    public class InvalidDataTypeConstraintException : Exception
    {
        public InvalidDataTypeConstraintException(string message) : base(message)
        {
        }
    }
}
