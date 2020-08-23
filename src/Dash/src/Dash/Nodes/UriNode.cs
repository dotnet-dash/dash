using System;
using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class UriNode : AstNode
    {
        public Uri Uri { get; }

        public UriNode(Uri uri, bool uriMustExist)
        {
            Uri = uri;
            UriMustExist = uriMustExist;
        }

        public bool UriMustExist { get; }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
