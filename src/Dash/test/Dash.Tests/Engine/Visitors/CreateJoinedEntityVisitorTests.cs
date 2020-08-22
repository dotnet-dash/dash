using System.Linq;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class CreateJoinedEntityVisitorTests
    {
        [Fact]
        public async Task Visit_HasAndBelongsToManyDeclarationNode()
        {
            // Arrange
            var sut = new CreateJoinedEntityVisitor(Substitute.For<IConsole>());

            var modelNode = new ModelNode();
            HasAndBelongsToManyDeclarationNode node = modelNode
                .AddEntityDeclarationNode("Foo")
                .AddHasAndBelongsToManyDeclarationNode("Bar", "Bar")
                .HasAndBelongsToMany
                .Last();

            // Act
            await sut.Visit(node);

            // Assert
            modelNode.EntityDeclarations.Should().SatisfyRespectively(
                first => first.Name.Should().Be("Foo"),
                second => second.Name.Should().Be("FooBar"));
        }
    }
}
