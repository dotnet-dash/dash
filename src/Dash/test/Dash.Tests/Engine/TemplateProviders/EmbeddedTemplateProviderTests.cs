// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using Dash.Engine.Providers;
using Dash.Exceptions;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.TemplateProviders
{
    public class EmbeddedTemplateProviderTests
    {
        [Fact]
        public void GetTemplate_EmbeddedResourceDoesNotExist_ShouldThrowEmbeddedTemplateNotFoundException()
        {
            // Arrange
            var sut = new EmbeddedTemplateProvider();

            // Act
            Action act = () => sut.GetTemplate("i do not exist");

            // Assert
            act.Should().Throw<EmbeddedTemplateNotFoundException>();
        }

        [Theory]
        [InlineData("efpoco")]
        [InlineData("EFPOCO")]
        public async Task GetTemplate_EmbeddedResourceExists_ShouldReturnContent(string templateName)
        {
            // Arrange
            var sut = new EmbeddedTemplateProvider();

            // Act
            var content = await sut.GetTemplate(templateName);

            // Assert
            content.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Exists_TemplateDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var sut = new EmbeddedTemplateProvider();

            // Act
            var result = await sut.Exists("i do not exist");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Exists_TemplateExists_ShouldReturnFalse()
        {
            // Arrange
            var sut = new EmbeddedTemplateProvider();

            // Act
            var result = await sut.Exists("efcore");

            // Assert
            result.Should().BeTrue();
        }
    }
}
