using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Dash.Tests
{
    public class ProgramTests
    {
        [Fact]
        public async Task Run_FileIsNull_ShouldHavePrintedErrorMessage()
        {
            // Arrange
            var textWriter = new StringWriter();

            Console.SetError(textWriter);

            // Act
            await Program.Main(null);

            // Assert
            textWriter.Close();
            textWriter.ToString().Should().Be("Please specify a model file." + Environment.NewLine);
        }
    }
}
