﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Dash.Engine;

namespace Dash.Nodes
{
    public class ConfigurationNode : AstNode
    {
        public string AutogenSuffix { get; set; } = ".generated";

        public string? Header { get; set; } = "<auto-generated>This code was generated by dotnet dash CLI. Manual changes to this file will be overwritten if the code is regenerated.</auto-generated>";

        public IList<TemplateNode> Templates { get; set; } = new List<TemplateNode>();

        public ConfigurationNode AddTemplateNode(string template, string? output)
        {
            var node = new TemplateNode
            {
                Template = template
            };

            if (output != null)
            {
                node.Output = output!;
            }

            Templates.Add(node);
            return this;
        }

        public ConfigurationNode AddTemplateNode(string template)
        {
            return AddTemplateNode(template, null);
        }

        public override async Task Accept(INodeVisitor visitor)
        {
            await visitor.Visit(this);
        }
    }
}
