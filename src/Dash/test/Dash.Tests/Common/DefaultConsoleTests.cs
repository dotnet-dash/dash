// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO;
using Dash.Application;
using Dash.Common.Default;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Dash.Tests.Common
{
    public class DefaultConsoleTests
    {
        [Fact]
        public void Trace_VerboseDisabled_ShouldNotWriteToConsole()
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
        public void Trace_VerboseEnabled_ShouldWriteToConsole()
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

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Info_VerboseDisabled_ShouldWriteToConsole(bool verbose)
        {
            // Arrange
            var sut = CreateSut(verbose);
            var textWriter = new StringWriter();
            Console.SetOut(textWriter);

            // Act
            sut.Info("Hello");

            // Assert
            textWriter.Close();
            textWriter.ToString().Should().Be("Hello" + Environment.NewLine);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Error_Verbose_ShouldWriteToConsole(bool verbose)
        {
            // Arrange
            var sut = CreateSut(verbose);
            var textWriter = new StringWriter();
            Console.SetError(textWriter);

            // Act
            sut.Error("Foo");

            // Assert
            textWriter.Close();
            textWriter.ToString().Should().Be("Foo" + Environment.NewLine);
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
