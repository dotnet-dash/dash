using Dash.Engine.Abstractions;
using Dash.Nodes;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Nodes
{
    public class HasAndBelongsToManyDeclarationNodeTests
    {
        [Fact]
        public void Accept_Visitor_VisitorShouldHaveCalledVisit()
        {
            // Arrange
            var sut = new HasAndBelongsToManyDeclarationNode(new EntityDeclarationNode(new ModelNode(), "Parent"), "Child", "Person");

            var visitor = Substitute.For<INodeVisitor>();

            // Act
            sut.Accept(visitor);

            // Assert
            visitor.Received(1).Visit(sut);
        }
    }
}