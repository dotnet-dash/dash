// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class EntityDeclarationNode : AstNode
    {
        private readonly List<AttributeDeclarationNode> _attributeDeclarations = new List<AttributeDeclarationNode>();
        private readonly List<InheritanceDeclarationNode> _inheritanceDeclarations = new List<InheritanceDeclarationNode>();
        private readonly List<AstNode> _astNodes = new List<AstNode>();

        public EntityDeclarationNode(ModelNode parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public ModelNode Parent { get; }

        public string Name { get; }

        public IEnumerable<AttributeDeclarationNode> AttributeDeclarations => _attributeDeclarations;

        public IEnumerable<InheritanceDeclarationNode> InheritanceDeclarationNodes => _inheritanceDeclarations;

        public IEnumerable<AbstractDeclarationNode> AbstractDeclarationNodes => _astNodes.OfType<AbstractDeclarationNode>();

        public IEnumerable<HasReferenceDeclarationNode> Has => _astNodes.OfType<HasReferenceDeclarationNode>();

        public IEnumerable<HasManyReferenceDeclarationNode> HasMany => _astNodes.OfType<HasManyReferenceDeclarationNode>();

        public IEnumerable<HasAndBelongsToManyDeclarationNode> HasAndBelongsToMany => _astNodes.OfType<HasAndBelongsToManyDeclarationNode>();

        public IList<AstNode> ChildNodes { get; } = new List<AstNode>();

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }

        public EntityDeclarationNode AddAttributeDeclaration(string attributeName, string attributeDataType)
        {
            var attribute = new AttributeDeclarationNode(this, attributeName, attributeDataType);
            _attributeDeclarations.Add(attribute);

            return this;
        }

        public void InsertAttributeDeclaration(int index, string attributeName, string attributeDataType)
        {
            var attribute = new AttributeDeclarationNode(this, attributeName, attributeDataType);
            _attributeDeclarations.Insert(index, attribute);
        }

        public InheritanceDeclarationNode AddInheritanceDeclaration(string inheritedEntity)
        {
            var inheritance = new InheritanceDeclarationNode(this, inheritedEntity);
            _inheritanceDeclarations.Add(inheritance);

            return inheritance;
        }

        public EntityDeclarationNode AddAbstractDeclarationNode(bool value)
        {
            _astNodes.Add(new AbstractDeclarationNode(this, value));
            return this;
        }

        public EntityDeclarationNode AddHasDeclaration(string name, string referencedEntity)
        {
            _astNodes.Add(new HasReferenceDeclarationNode(this, name, referencedEntity));
            return this;
        }

        public EntityDeclarationNode AddHasManyDeclaration(string name, string referencedEntity)
        {
            _astNodes.Add(new HasManyReferenceDeclarationNode(this, name, referencedEntity));
            return this;
        }

        public EntityDeclarationNode AddHasAndBelongsToManyDeclarationNode(string name, string referencedEntity)
        {
            _astNodes.Add(new HasAndBelongsToManyDeclarationNode(this, name, referencedEntity));
            return this;
        }

        public void AddCsvSeedDeclarationNode(Uri uri, bool firstLineIsHeader, string? delimiter, IDictionary<string, string> mapHeaders)
        {
            ChildNodes.Add(new CsvSeedDeclarationNode(this, uri, firstLineIsHeader, delimiter, mapHeaders));
        }
    }
}