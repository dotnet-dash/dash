using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Nodes;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Nodes
{
    public class CsvSeedDeclarationNodeTests
    {
        [Fact]
        public async Task Accept_Visitor_SutShouldHaveCalledVisit()
        {
            // Arrange
            var sut = new CsvSeedDeclarationNode(default, default, default, default, default);

            var visitor = Substitute.For<INodeVisitor>();

            // Act
            await sut.Accept(visitor);

            // Assert
            await visitor.Received(1).Visit(sut);
        }
    }
}