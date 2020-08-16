using Dash.Engine.Abstractions;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine
{
    public class DefaultSymbolCollectorTests
    {
        [Fact]
        public void GetEntityNames_NoVisits_ShouldReturnEmptyCollection()
        {
            // Arrange
            var sut = new DefaultSymbolCollector(Substitute.For<IConsole>());

            // Act
            var result = sut.GetEntityNames();

            // Assert
            result.Count.Should().Be(0);
        }


        [Fact]
        public void GetEntityNames_EntityDeclarationNodeVisited_ShouldReturnEntityNames()
        {
            // Arrange
            var sut = new DefaultSymbolCollector(Substitute.For<IConsole>());

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

        [Theory]
        [InlineData("Account", true)]
        [InlineData("Order", true)]
        [InlineData("OrderLine", true)]
        [InlineData("Accounts", false)]
        public void EntityExists_GivenEntityName_ShouldReturnExpectedResult(string entityName, bool expectedResult)
        {
            // Arrange
            var sut = new DefaultSymbolCollector(Substitute.For<IConsole>());

            sut.Visit(new EntityDeclarationNode("Account"));
            sut.Visit(new EntityDeclarationNode("Order"));
            sut.Visit(new EntityDeclarationNode("OrderLine"));

            // Act
            var result = sut.EntityExists(entityName);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
