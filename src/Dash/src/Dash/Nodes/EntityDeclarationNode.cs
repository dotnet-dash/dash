using System.Collections.Generic;
using Dash.Engine.Abstractions;

namespace Dash.Nodes
{
    public class EntityDeclarationNode : AstNode
    {
        private readonly List<AttributeDeclarationNode> _attributeDeclarations = new List<AttributeDeclarationNode>();

        public EntityDeclarationNode(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string? InheritedEntity { get; set; }

        public IEnumerable<AttributeDeclarationNode> AttributeDeclarations => _attributeDeclarations;

        public IList<HasReferenceDeclarationNode> Has { get; } = new List<HasReferenceDeclarationNode>();

        public IList<HasManyReferenceDeclarationNode> HasMany { get; } = new List<HasManyReferenceDeclarationNode>();

        public IList<HasAndBelongsToManyDeclarationNode> HasAndBelongsToMany { get; } = new List<HasAndBelongsToManyDeclarationNode>();

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void AddAttributeDeclaration(string attributeName, string attributeDataType)
        {
            var attribute = new AttributeDeclarationNode(this, attributeName, attributeDataType);
            _attributeDeclarations.Add(attribute);
        }

        public void AddHasDeclaration(string name, string referencedEntity)
        {
            var has = new HasReferenceDeclarationNode(this, name, referencedEntity);
            Has.Add(has);
        }
    }
}