using System.Linq;
using Dash.Engine;
using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine
{
    public class DefaultSemanticAnalyzerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Analyze_NullOrEmptyName_ShouldProduceError(string entityName)
        {
            // Arrange
            var sut = new DefaultSemanticAnalyzer();

            var model = new Model();
            model.Entities.Add(new Entity(entityName) { Inherits = null });

            // Act
            var result = sut.Analyze(model).ToList();

            // Assert
            result.Count().Should().Be(1);
            result.Should().Contain("Entity Name Cannot Be Null, Empty, or Whitespaces");
        }

        [Fact]
        public void Analyze_EntityDeclaredTwoTimes_ShouldProduceError()
        {
            // Arrange
            var sut = new DefaultSemanticAnalyzer();

            var model = new Model();
            model.Entities.Add(new Entity("Hello") { Inherits = null });
            model.Entities.Add(new Entity("Hello") { Inherits = null });

            // Act
            var result = sut.Analyze(model).ToList();

            // Assert
            result.Count().Should().Be(1);
            result.Should().Contain("Entity Declared Multiple Times: 'Hello' (2 times)");
        }

        [Fact]
        public void Analyze_InheritUnknownEntity_ShouldProduceError()
        {
            // Arrange
            var sut = new DefaultSemanticAnalyzer();

            var model = new Model();
            model.Entities.Add(
                new Entity("MyEntity")
                {
                    Inherits = "SuperEntity"
                }
            );

            // Act
            var result = sut.Analyze(model).ToList();

            // Assert
            result.Count().Should().Be(1);
            result.Should().Contain("Unknown Entity Inheritance: 'MyEntity' wants to inherit unknown entity 'SuperEntity'");
        }

        [Fact]
        public void Analyze_EntityInheritingItself_ShouldProduceError()
        {
            // Arrange
            var sut = new DefaultSemanticAnalyzer();

            var model = new Model();
            model.Entities.Add(
                new Entity("MyEntity")
                {
                    Inherits = "MyEntity"
                }
            );

            // Act
            var result = sut.Analyze(model).ToList();

            // Assert
            result.Count().Should().Be(1);
            result.Should().Contain("Self-Inheritance Not Allowed: 'MyEntity'");
        }
    }
}