// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Common;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class DefaultSymbolCollector : BaseVisitor
    {
        private readonly ISymbolRepository _symbolRepository;

        public DefaultSymbolCollector(IConsole console, ISymbolRepository symbolRepository) : base(console)
        {
            _symbolRepository = symbolRepository;
        }

        public override Task Visit(EntityDeclarationNode node)
        {
            _symbolRepository.AddEntity(node.Name);
            Console.Trace($"Adding symbol: {node.Name}");

            return base.Visit(node);
        }

        public override Task Visit(AttributeDeclarationNode node)
        {
            _symbolRepository.AddEntityAttribute(node.Parent.Name, node.AttributeName);

            return base.Visit(node);
        }
    }
}
