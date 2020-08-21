using System;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;

namespace Dash.Nodes
{
    public class UriNode : AstNode
    {
        public Uri Uri { get; }

        public UriNode(Uri uri)
        {
            Uri = uri;
        }

        public string? LocalCopy { get; set; }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
