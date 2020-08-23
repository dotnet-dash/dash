using System.Threading.Tasks;
using Dash.Engine;
using Dash.Nodes;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Nodes
{
    public class InheritanceDeclarationNodeTests
    {
        [Fact]
        public async Task Accept_Visitor_VisitorShouldHaveCalledVisit()
        {
            // Arrange
            var sut = new InheritanceDeclarationNode(new EntityDeclarationNode(new ModelNode(), "Account"), "Base");

            var visitor = Substitute.For<INodeVisitor>();

            // Act
            await sut.Accept(visitor);

            // Assert
            await visitor.Received(1).Visit(sut);
        }
    }
}
