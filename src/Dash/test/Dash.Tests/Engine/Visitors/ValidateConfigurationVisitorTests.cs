// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine.Repositories;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class ValidateConfigurationVisitorTests
    {
        [Fact]
        public async  Task Visit_TemplatesObjectHasNoTemplateProperty_ShouldAddErrorToRepository()
        {
            // Arrange
            var errorRepository = new ErrorRepository();

            var sut = new ValidateConfigurationVisitor(NSubstitute.Substitute.For<IConsole>(), errorRepository);

            var node = new ConfigurationNode();
            node.Templates.Add(new TemplateNode
            {
                Output = "."
            });

            // Act
            await sut.Visit(node);

            // Assert
            errorRepository.GetErrors().Should().SatisfyRespectively(first =>
            {
                first.Should().Be("Configuration.Templates[0] object has no 'Template' property.");
            });
        }
    }
}
