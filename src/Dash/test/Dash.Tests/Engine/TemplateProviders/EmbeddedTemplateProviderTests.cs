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
    }
}
