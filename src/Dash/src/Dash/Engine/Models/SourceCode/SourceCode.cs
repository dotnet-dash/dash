using Dash.Nodes;

namespace Dash.Engine.Models.SourceCode
{
    public class SourceCode
    {
        public SourceCode(Configuration configuration, ModelNode modelNode)
        {
            Configuration = configuration;
            ModelNode = modelNode;
        }

        public Configuration Configuration { get; set; }

        public ModelNode ModelNode { get; set; }
    }
}
