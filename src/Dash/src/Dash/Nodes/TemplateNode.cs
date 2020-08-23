using System;
using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class TemplateNode : AstNode
    {
        public Uri? Template { get; set; }

        public Uri Output { get; set; } = new Uri("file:///relative");

        public UriNode? TemplateUriNode
        {
            get
            {
                if (Template == null)
                {
                    return null;
                }

                try
                {
                    return new UriNode(Template, true);
                }
                catch (UriFormatException)
                {
                    return null;
                }
            }
        }

        public UriNode? OutputUriNode
        {
            get
            {
                try
                {
                    return new UriNode(Output, false);
                }
                catch (UriFormatException)
                {
                    return null;
                }
            }
        }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}