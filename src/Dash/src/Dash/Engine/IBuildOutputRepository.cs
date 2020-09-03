using System.Collections.Generic;

namespace Dash.Engine
{
    public interface IBuildOutputRepository
    {
        void Add(string path, string content);

        void Update(BuildOutput item);

        IEnumerable<BuildOutput> GetOutputItems();
    }
}