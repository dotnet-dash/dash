// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class SourceCodeNode : AstNode
    {
        public SourceCodeNode(ConfigurationNode configurationNode, ModelNode modelNode)
        {
            ConfigurationNode = configurationNode;
            ModelNode = modelNode;
        }

        public ConfigurationNode ConfigurationNode { get; }

        public ModelNode ModelNode { get; }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
