using System;
using System.Runtime.Serialization;

namespace Dash.Exceptions
{
    [Serializable]
    public class EntityModelNotFoundException : Exception
    {
        public EntityModelNotFoundException(string errorMessage) : base(errorMessage)
        {
        }

        protected EntityModelNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
