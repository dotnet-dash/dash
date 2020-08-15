using Dash.Engine;
using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine
{
    public class DefaultSymbolCollectorTests
    {
        [Fact]
        public void GetEntityNames_NoVisits_ShouldReturnEmptyCollection()
        {
            // Arrange
            var sut = new DefaultSymbolCollector();

            // Act
            var result = sut.GetEntityNames();

            // Assert
            result.Count.Should().Be(0);
        }


        [Fact]
        public void GetEntityNames_EntityDeclarationNodeVisited_ShouldReturnEntityNames()
        {
            // Arrange
            var sut = new DefaultSymbolCollector();

            sut.Visit(new EntityDeclarationNode("Account"));
            sut.Visit(new EntityDeclarationNode("Order"));
            sut.Visit(new EntityDeclarationNode("OrderLine"));

            // Act
            var result = sut.GetEntityNames();

            // Assert
            result.Should().SatisfyRespectively(
                first => first.Should().Be("Account"),
                second => second.Should().Be("Order"),
                third => third.Should().Be("OrderLine")
            );
        }
    }
}
