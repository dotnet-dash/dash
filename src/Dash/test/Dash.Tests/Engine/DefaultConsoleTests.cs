using System;
using System.IO;
using Dash.Application;
using Dash.Common;
using Dash.Engine;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Dash.Tests.Engine
{
    public class DefaultConsoleTests
    {
        [Fact]
        public void Trace_VerboseDisabled_ShouldNotHaveWrittenToTheConsole()
        {
            // Arrange
            var sut = CreateSut(false);
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);

            // Act
            sut.Trace("Hello");

            // Assert
            textWriter.Close();
            textWriter.ToString().Should().Be(string.Empty);
        }

        [Fact]
        public void Trace_VerboseEnabled_ShouldHaveWrittenToConsole()
        {
            // Arrange
            var sut = CreateSut(true);
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);

            // Act
            sut.Trace("Hello");

            // Assert
            textWriter.Close();
            textWriter.ToString().Should().Be("Hello" + Environment.NewLine);
        }

        [Fact]
        public void Info_VerboseDisabled_ShouldHaveWrittenToConsole()
        {
            // Arrange
            var sut = CreateSut(false);
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);

            // Act
            sut.Info("Hello");

            // Assert
            textWriter.Close();
            textWriter.ToString().Should().Be("Hello" + Environment.NewLine);
        }

        private static DefaultConsole CreateSut(bool verbose)
        {
            return new DefaultConsole(new OptionsWrapper<DashOptions>(new DashOptions()
            {
                Verbose = verbose
            }));
        }
    }
}
