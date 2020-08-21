using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Nodes;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Nodes
{
    public class ModelNodeTests
    {
        [Fact]
        public async Task Accept_Visitor_SutShouldHaveCalledVisit()
        {
            // Arrange
            var sut = new ModelNode();

            var visitor = Substitute.For<INodeVisitor>();

            // Act
            await sut.Accept(visitor);

            // Assert
            await visitor.Received(1).Visit(sut);
        }
    }
}
