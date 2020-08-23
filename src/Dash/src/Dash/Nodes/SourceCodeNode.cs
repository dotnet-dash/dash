using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class SourceCodeNode : AstNode
    {
        public SourceCodeNode(ConfigurationNode configurationNode, ModelNode modelNode)
        {
            ConfigurationNode = configurationNode;
            ModelNode = modelNode;
        }

        public ConfigurationNode ConfigurationNode { get; }

        public ModelNode ModelNode { get; }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
