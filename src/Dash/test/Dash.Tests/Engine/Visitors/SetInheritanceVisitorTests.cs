using System.Threading.Tasks;
using Dash.Common.Abstractions;
using Dash.Engine.Abstractions;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class SetInheritanceVisitorTests
    {
        [Fact]
        public async Task Visit_ModelNode_ShouldHaveSetDefaultInheritanceIfNotDeclared()
        {
            // Arrange
            var sut = new SetInheritanceVisitor(NSubstitute.Substitute.For<IConsole>());

            ModelNode modelNode = new ModelNode();
            modelNode.AddEntityDeclarationNode("Order");
            modelNode.AddEntityDeclarationNode("OrderLine").AddInheritanceDeclaration("SomeOtherBase");

            // Act
            await sut.Visit(modelNode);

            // Assert
            modelNode.EntityDeclarations.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Order");
                    first.InheritanceDeclarationNodes.Should().SatisfyRespectively(
                        a =>
                        {
                            a.InheritedEntity.Should().Be("Base");
                        });
                },
                second =>
                {
                    second.Name.Should().Be("OrderLine");
                    second.InheritanceDeclarationNodes.Should().SatisfyRespectively(
                        a =>
                        {
                            a.InheritedEntity.Should().Be("SomeOtherBase");
                        });
                },
                third =>
                {
                    third.Name.Should().Be("Base");
                    third.InheritanceDeclarationNodes.Should().BeEmpty();
                });
        }
    }
}
