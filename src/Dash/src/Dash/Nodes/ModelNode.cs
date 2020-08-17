using System.Collections.Generic;
using Dash.Engine.Abstractions;

namespace Dash.Nodes
{
    public class ModelNode : AstNode
    {
        public IList<EntityDeclarationNode> EntityDeclarations { get; } = new List<EntityDeclarationNode>();

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public EntityDeclarationNode AddEntityDeclarationNode(string name)
        {
            var node = new EntityDeclarationNode(this, name);
            EntityDeclarations.Add(node);

            return node;
        }
    }
}