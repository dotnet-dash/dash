using System.Collections.Generic;
using Dash.Engine.Models;
using Dash.Engine.Models.SourceCode;

namespace Dash.Nodes
{
    public class Model
    {
        public Configuration Configuration { get; set; } = new Configuration();

        public IEnumerable<EntityModel> Entities { get; set; } = new List<EntityModel>();
    }
}
