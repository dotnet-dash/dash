using System;
using System.Runtime.Serialization;

namespace Dash.Exceptions
{
    [Serializable]
    public class EmbeddedTemplateNotFoundException : Exception
    {
        public EmbeddedTemplateNotFoundException(string message) : base(message)
        {
        }

        protected EmbeddedTemplateNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
