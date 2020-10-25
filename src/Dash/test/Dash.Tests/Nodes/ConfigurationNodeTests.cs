using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Nodes
{
    public class ConfigurationNodeTests
    {
        [Fact]
        public void Pluralize_CtorCalled_ShouldReturnTrue()
        {
            // Arrange
            var sut = new ConfigurationNode();

            // Act
            var result = sut.Pluralize;

            // Assert
            result.Should().BeTrue();
        }
    }
}
