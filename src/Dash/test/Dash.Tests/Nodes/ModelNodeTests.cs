using Dash.Engine.Abstractions;
using Dash.Nodes;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Nodes
{
    public class ModelNodeTests
    {
        [Fact]
        public void Accept_Visitor_SutShouldHaveCalledVisit()
        {
            // Arrange
            var sut = new ModelNode();

            var visitor = Substitute.For<INodeVisitor>();

            // Act
            sut.Accept(visitor);

            // Assert
            visitor.Received(1).Visit(sut);
        }
    }
}
