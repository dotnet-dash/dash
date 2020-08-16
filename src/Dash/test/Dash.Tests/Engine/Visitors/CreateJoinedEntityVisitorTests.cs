using Dash.Engine.Abstractions;
using Dash.Engine.Visitors;
using Dash.Nodes;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class CreateJoinedEntityVisitorTests
    {
        [Fact]
        public void Visit_HasAndBelongsToManyDeclarationNode()
        {
            // Arrange
            var sut = new CreateJoinedEntityVisitor(Substitute.For<IConsole>());

            var parent = new EntityDeclarationNode(new ModelNode(), "Order");
            var node = new HasAndBelongsToManyDeclarationNode(parent, "Order", "Order");

            // Act
            sut.Visit(node);

            // Assert
        }
    }
}
