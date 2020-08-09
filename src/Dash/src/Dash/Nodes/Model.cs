using System.Collections.Generic;

namespace Dash.Nodes
{
    public class Model
    {
        public IList<string> Errors { get; } = new List<string>();
        public IList<Entity> Entities { get; } = new List<Entity>();
    }
}
