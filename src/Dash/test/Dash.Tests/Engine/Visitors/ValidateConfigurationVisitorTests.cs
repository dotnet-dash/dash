// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Engine;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentArrange.NSubstitute;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class ValidateConfigurationVisitorTests
    {
        [Fact]
        public async  Task Visit_TemplatesObjectHasNoTemplateProperty_ShouldCallErrorRepositoryAdd()
        {
            // Arrange
            var context = Arrange.For<ValidateConfigurationVisitor>();

            var node = new ConfigurationNode();
            node.Templates.Add(new TemplateNode
            {
                Output = "."
            });

            // Act
            await context.Sut.Visit(node);

            // Assert
            context.Dependency<IErrorRepository>().Received(1).Add("Configuration.Templates[0] object has no 'Template' property.");
            context.Dependency<IErrorRepository>().ReceivedWithAnyArgs(1);
        }
    }
}
