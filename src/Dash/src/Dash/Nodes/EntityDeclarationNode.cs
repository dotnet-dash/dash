using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;

namespace Dash.Nodes
{
    public class EntityDeclarationNode : AstNode
    {
        private readonly List<AttributeDeclarationNode> _attributeDeclarations = new List<AttributeDeclarationNode>();
        private readonly List<InheritanceDeclarationNode> _inheritanceDeclarations = new List<InheritanceDeclarationNode>();

        public EntityDeclarationNode(ModelNode parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public ModelNode Parent { get; set; }

        public string Name { get; }

        public IEnumerable<AttributeDeclarationNode> AttributeDeclarations => _attributeDeclarations;

        public IEnumerable<InheritanceDeclarationNode> InheritanceDeclarationNodes => _inheritanceDeclarations;

        public IList<HasReferenceDeclarationNode> Has { get; } = new List<HasReferenceDeclarationNode>();

        public IList<HasManyReferenceDeclarationNode> HasMany { get; } = new List<HasManyReferenceDeclarationNode>();

        public IList<HasAndBelongsToManyDeclarationNode> HasAndBelongsToMany { get; } = new List<HasAndBelongsToManyDeclarationNode>();

        public IList<AstNode> ChildNodes { get; } = new List<AstNode>();

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }

        public void AddAttributeDeclaration(string attributeName, string attributeDataType)
        {
            var attribute = new AttributeDeclarationNode(this, attributeName, attributeDataType);
            _attributeDeclarations.Add(attribute);
        }

        public void InsertAttributeDeclaration(int index, string attributeName, string attributeDataType)
        {
            var attribute = new AttributeDeclarationNode(this, attributeName, attributeDataType);
            _attributeDeclarations.Insert(index, attribute);
        }

        public void AddInheritanceDeclaration(string inheritedEntity)
        {
            var inheritance = new InheritanceDeclarationNode(this, inheritedEntity);
            _inheritanceDeclarations.Add(inheritance);
        }

        public void AddHasDeclaration(string name, string referencedEntity)
        {
            var has = new HasReferenceDeclarationNode(this, name, referencedEntity);
            Has.Add(has);
        }

        public void AddCsvSeedDeclarationNode(Uri uri, bool firstLineIsHeader, string? delimiter, IDictionary<string, string> mapHeaders)
        {
            ChildNodes.Add(new CsvSeedDeclarationNode(this, uri, firstLineIsHeader, delimiter, mapHeaders));
        }
    }
}