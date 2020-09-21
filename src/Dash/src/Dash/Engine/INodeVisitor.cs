// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Nodes;

namespace Dash.Engine
{
    public interface INodeVisitor
    {
        Task Visit(SourceCodeNode node);
        Task Visit(ConfigurationNode node);
        Task Visit(TemplateNode node);
        Task Visit(ModelNode node);
        Task Visit(EntityDeclarationNode node);
        Task Visit(AttributeDeclarationNode node);
        Task Visit(HasReferenceDeclarationNode node);
        Task Visit(HasManyReferenceDeclarationNode node);
        Task Visit(HasAndBelongsToManyDeclarationNode node);
        Task Visit(InheritanceDeclarationNode node);
        Task Visit(AbstractDeclarationNode node);
        Task Visit(CsvSeedDeclarationNode node);
        Task Visit(UriNode node);
    }
}