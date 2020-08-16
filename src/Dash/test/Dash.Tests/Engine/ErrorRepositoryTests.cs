using Dash.Engine;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine
{
    public class ErrorRepositoryTests
    {
        [Fact]
        public void HasErrors_NoErrorsAdded_ShouldReturnFalse()
        {
            // Arrange
            var sut = new ErrorRepository();

            // Act
            var result = sut.HasErrors();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void HasErrors_ErrorAdded_ShouldReturnTrue()
        {
            // Arrange
            var sut = new ErrorRepository();
            sut.Add("Error");

            // Act
            var result = sut.HasErrors();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GetErrors_ErrorAdded_ShouldReturnError()
        {
            // Arrange
            var sut = new ErrorRepository();
            sut.Add("First error");
            sut.Add("Second error");

            // Act
            var result = sut.GetErrors();

            // Assert
            result.Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("First error");
                },
                second =>
                {
                    second.Should().Be("Second error");
                }
            );
        }
    }
}
