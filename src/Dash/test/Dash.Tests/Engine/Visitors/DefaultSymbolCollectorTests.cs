using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine;
using Dash.Engine.Repositories;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class DefaultSymbolCollectorTests
    {
        private readonly ISymbolRepository _symbolRepository = new DefaultSymbolRepository();
        private readonly DefaultSymbolCollector _sut;

        public DefaultSymbolCollectorTests()
        {
            _sut = new DefaultSymbolCollector(Substitute.For<IConsole>(), _symbolRepository);
        }

        [Fact]
        public void GetEntityNames_NoVisits_ShouldReturnEmptyCollection()
        {
            // Act
            var result = _symbolRepository.GetEntityNames();

            // Assert
            result.Count.Should().Be(0);
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_ShouldAddSymbolsToRepository()
        {
            // Arrange
            var modelNode = new ModelNode();

            // Act
            await _sut.Visit(new EntityDeclarationNode(modelNode, "Foo"));

            // Assert
            _symbolRepository
                .GetEntityNames()
                .Should().SatisfyRespectively(first => first.Should().Be("Foo"));
        }

        [Fact]
        public async Task Visit_ModelNode_ShouldAddAllSymbolsToRepository()
        {
            // Arrange
            var modelNode = new ModelNode();
            modelNode
                .AddEntityDeclarationNode("Order")
                .AddAttributeDeclaration("Description", "String");

            modelNode
                .AddEntityDeclarationNode("OrderLine")
                .AddAttributeDeclaration("Quantity", "Int");

            // Act
            await _sut.Visit(modelNode);

            // Assert
            _symbolRepository
                .GetAttributeNames("Order")
                .Should().SatisfyRespectively(first => first.Should().Be("Description"));

            _symbolRepository
                .GetAttributeNames("OrderLine")
                .Should().SatisfyRespectively(first => first.Should().Be("Quantity"));
        }
    }
}
