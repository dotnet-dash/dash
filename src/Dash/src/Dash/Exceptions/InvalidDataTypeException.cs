using System;

namespace Dash.Exceptions
{
    public class InvalidDataTypeException : Exception
    {
        public InvalidDataTypeException(string specifiedDashDataType) : base($"The specified datatype '{specifiedDashDataType}' is invalid")
        {
        }
    }
}
