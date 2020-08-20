using System;
using System.Runtime.Serialization;

namespace Dash.Exceptions
{
    [Serializable]
    public sealed class EntityModelNotFoundException : Exception
    {
        public EntityModelNotFoundException(string errorMessage) : base(errorMessage)
        {
        }

        private EntityModelNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
