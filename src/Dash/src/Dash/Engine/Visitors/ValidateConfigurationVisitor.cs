// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Common;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class ValidateConfigurationVisitor : BaseVisitor
    {
        private readonly IErrorRepository _errorRepository;

        public ValidateConfigurationVisitor(IConsole console, IErrorRepository errorRepository) : base(console)
        {
            _errorRepository = errorRepository;
        }

        public override async Task Visit(ConfigurationNode node)
        {
            for (var counter = 0; counter < node.Templates.Count; counter++)
            {
                if (node.Templates[counter].Template == null)
                {
                    _errorRepository.Add($"Configuration.Templates[{counter}] object has no 'Template' property.");
                }
            }

            await base.Visit(node);
        }
    }
}
