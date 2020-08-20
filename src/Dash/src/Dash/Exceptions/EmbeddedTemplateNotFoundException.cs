using System;
using System.Runtime.Serialization;

namespace Dash.Exceptions
{
    [Serializable]
    public sealed class EmbeddedTemplateNotFoundException : Exception
    {
        public EmbeddedTemplateNotFoundException(string message) : base(message)
        {
        }

        private EmbeddedTemplateNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
