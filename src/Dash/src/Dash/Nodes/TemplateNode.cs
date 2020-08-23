using System.Threading.Tasks;
using Dash.Engine;
using Dash.Extensions;

namespace Dash.Nodes
{
    public class TemplateNode : AstNode
    {
        public string? Template { get; set; } = string.Empty;

        public string Output { get; set; } = ".";

        public UriNode? TemplateUriNode
        {
            get
            {
                var uri = Template?.ToUri();
                return uri == null
                    ? null
                    : new UriNode(uri, true);
            }
        }

        public UriNode? OutputUriNode
        {
            get
            {
                var uri = Output.ToUri();
                return uri == null
                    ? null
                    : new UriNode(uri, false);
            }
        }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}