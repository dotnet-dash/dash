using System.Collections.Generic;
using System.Linq;
using Dash.Engine.Abstractions;

namespace Dash.Engine
{
    public class ErrorRepository : IErrorRepository
    {
        private readonly List<string> _errors = new List<string>();

        public bool HasErrors()
        {
            return _errors.Any();
        }

        public IEnumerable<string> GetErrors()
        {
            return _errors;
        }

        public void Add(string error)
        {
            _errors.Add(error);
        }
    }
}