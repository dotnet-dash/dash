using System.Collections.Generic;
using Dash.Engine.Abstractions;

namespace Dash.Nodes
{
    public class EntityDeclarationNode : AstNode
    {
        private List<AttributeDeclarationNode> _attributeDeclarations = new List<AttributeDeclarationNode>();

        public EntityDeclarationNode(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<AttributeDeclarationNode> AttributeDeclarations => _attributeDeclarations;

        public IList<ReferenceDeclarationNode> SingleEntityReferences { get; } = new List<ReferenceDeclarationNode>();

        public IList<ReferenceDeclarationNode> CollectionEntityReferences { get; } = new List<ReferenceDeclarationNode>();

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void AddAttributeDeclaration(string attributeName, string attributeDataType)
        {
            var attribute = new AttributeDeclarationNode(this, attributeName, attributeDataType);
            _attributeDeclarations.Add(attribute);
        }
    }
}