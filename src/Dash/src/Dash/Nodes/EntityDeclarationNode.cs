using System.Collections.Generic;

namespace Dash.Nodes
{
    public class EntityDeclarationNode : AstNode
    {
        public EntityDeclarationNode(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IList<AttributeDeclarationNode> AttributeDeclarations { get; } = new List<AttributeDeclarationNode>();

        public IList<ReferenceDeclarationNode> SingleEntityReferences { get; } = new List<ReferenceDeclarationNode>();

        public IList<ReferenceDeclarationNode> CollectionEntityReferences { get; } = new List<ReferenceDeclarationNode>();

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}