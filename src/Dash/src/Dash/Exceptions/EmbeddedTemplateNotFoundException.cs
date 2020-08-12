using System;

namespace Dash.Exceptions
{
    public class EmbeddedTemplateNotFoundException : Exception
    {
        public EmbeddedTemplateNotFoundException(string message) : base(message)
        {
        }
    }
}
