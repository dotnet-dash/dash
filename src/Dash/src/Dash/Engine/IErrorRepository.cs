using System.Collections.Generic;

namespace Dash.Engine
{
    public interface IErrorRepository
    {
        void Add(string error);
        bool HasErrors();
        IEnumerable<string> GetErrors();
    }
}
