using System;
using Dash.Engine;
using Dash.Engine.Repositories;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Repositories
{
    public class BuildOutputRepositoryTests
    {
        [Fact]
        public void Add_NotAdded_ShouldAdd()
        {
            // Arrange
            var sut = new BuildOutputRepository();

            // Act
            sut.Add("c:/foo.cs", "Foo");

            // Assert
            sut.GetOutputItems().Should().SatisfyRespectively(first =>
            {
                first.Path.Should().Be("c:/foo.cs");
                first.GeneratedSourceCodeContent.Should().Be("Foo");
            });
        }

        [Fact]
        public void Add_Added_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var sut = new BuildOutputRepository();
            sut.Add("c:/foo.cs", "Foo");

            // Act
            Action act = () => sut.Add("c:/foo.cs", "Foobar");

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Update_NotAdded_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var sut = new BuildOutputRepository();

            // Act
            Action act = () => sut.Update(new BuildOutput("c:/foo.cs", "Bar"));

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Update_Added_ShouldUpdate()
        {
            // Arrange
            var sut = new BuildOutputRepository();
            sut.Add("c:/foo.cs", "Foo");

            // Act
            sut.Update(new BuildOutput("c:/foo.cs", "Bar"));

            // Assert
            sut.GetOutputItems().Should().SatisfyRespectively(first =>
            {
                first.Path.Should().Be("c:/foo.cs");
                first.GeneratedSourceCodeContent.Should().Be("Bar");
            });
        }

        [Fact]
        public void GetOutputItems_MultipleItems_ShouldReturnItems()
        {
            // Arrange
            var sut = new BuildOutputRepository();
            sut.Add("c:/foo.cs", "Foo");
            sut.Add("c:/bar.cs", "Bar");

            // Act
            var result = sut.GetOutputItems();

            // Assert
            result.Should().SatisfyRespectively(first =>
            {
                first.Path.Should().Be("c:/foo.cs");
                first.GeneratedSourceCodeContent.Should().Be("Foo");
            }, second =>
            {
                second.Path.Should().Be("c:/bar.cs");
                second.GeneratedSourceCodeContent.Should().Be("Bar");
            });
        }
    }
}
