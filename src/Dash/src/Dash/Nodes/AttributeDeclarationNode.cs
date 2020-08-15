using System.ComponentModel;
using Dash.Engine.Abstractions;

namespace Dash.Nodes
{
    public class AttributeDeclarationNode : AstNode
    {
        public AttributeDeclarationNode(EntityDeclarationNode parent, string attributeName, string attributeDataType)
        {
            Parent = parent;
            AttributeName = attributeName;
            AttributeDataType = attributeDataType;
        }

        public EntityDeclarationNode Parent { get; set; }

        public string AttributeName { get; set; }

        public string AttributeDataType { get; set; }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}