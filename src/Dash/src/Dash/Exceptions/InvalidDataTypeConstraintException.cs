using System;
using System.Runtime.Serialization;

namespace Dash.Exceptions
{
    [Serializable]
    public class InvalidDataTypeConstraintException : Exception
    {
        public InvalidDataTypeConstraintException(string message) : base(message)
        {
        }

        protected InvalidDataTypeConstraintException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
