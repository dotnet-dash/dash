using System;
using System.Runtime.Serialization;

namespace Dash.Exceptions
{
    [Serializable]
    public sealed class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        {
        }

        private ParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
