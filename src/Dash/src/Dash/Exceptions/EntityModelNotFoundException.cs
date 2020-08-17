using System;

namespace Dash.Exceptions
{
    public class EntityModelNotFoundException : Exception
    {
        public EntityModelNotFoundException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
