using System;
using System.Runtime.Serialization;

namespace Dash.Exceptions
{
    [Serializable]
    public sealed class InvalidDataTypeConstraintException : Exception
    {
        public InvalidDataTypeConstraintException(string message) : base(message)
        {
        }

        private InvalidDataTypeConstraintException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
