using Dash.Engine.Abstractions;
using Dash.Nodes;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Nodes
{
    public class InheritanceDeclarationNodeTests
    {
        [Fact]
        public void Accept_Visitor_VisitorShouldHaveCalledVisit()
        {
            // Arrange
            var sut = new InheritanceDeclarationNode(new EntityDeclarationNode(new ModelNode(), "Account"), "Base");

            var visitor = Substitute.For<INodeVisitor>();

            // Act
            sut.Accept(visitor);

            // Assert
            visitor.Received(1).Visit(sut);
        }
    }
}
