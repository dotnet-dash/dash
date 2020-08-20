using System;
using System.Runtime.Serialization;

namespace Dash.Exceptions
{
    [Serializable]
    public sealed class InvalidDataTypeException : Exception
    {
        public InvalidDataTypeException(string specifiedDashDataType) : base($"The specified datatype '{specifiedDashDataType}' is invalid")
        {
        }

        private InvalidDataTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
