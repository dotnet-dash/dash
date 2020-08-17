using System.Collections.Generic;

namespace Dash.Engine.Abstractions
{
    public interface IErrorRepository
    {
        void Add(string error);
        bool HasErrors();
        IEnumerable<string> GetErrors();
    }
}
