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

            var modelNode = new ModelNode();

            sut.Visit(new EntityDeclarationNode(modelNode, "Account"));
            sut.Visit(new EntityDeclarationNode(modelNode, "Order"));
            sut.Visit(new EntityDeclarationNode(modelNode, "OrderLine"));

            // Act
            var result = sut.GetEntityNames();

            // Assert
            result.Should().SatisfyRespectively(
                first => first.Should().Be("Account"),
                second => second.Should().Be("Order"),
                third => third.Should().Be("OrderLine")
            );
        }

        [Fact]
        public void GetAttributeNames_EntityNameNotFound_ShouldReturnEmptyList()
        {
            // Arrange
            var sut = new DefaultSymbolCollector(Substitute.For<IConsole>());

            // Act
            var result = sut.GetAttributeNames("Order");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetAttributeNames_EntityNameFound_ShouldReturnEntityAttributeNames()
        {
            // Arrange
            var sut = new DefaultSymbolCollector(Substitute.For<IConsole>());

            var modelNode = new ModelNode();
            var orderNode = modelNode.AddEntityDeclarationNode("Order");
            orderNode.AddAttributeDeclaration("Description", "String");

            var orderLineNode = modelNode.AddEntityDeclarationNode("OrderLine");
            orderLineNode.AddAttributeDeclaration("Quantity", "Int");

            sut.Visit(modelNode);

            // Act
            var result = sut.GetAttributeNames("Order");

            // Assert
            result.Should().SatisfyRespectively(
                first => first.Should().Be("Description"));
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

            var modelNode = new ModelNode();

            sut.Visit(new EntityDeclarationNode(modelNode, "Account"));
            sut.Visit(new EntityDeclarationNode(modelNode, "Order"));
            sut.Visit(new EntityDeclarationNode(modelNode, "OrderLine"));

            // Act
            var result = sut.EntityExists(entityName);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
