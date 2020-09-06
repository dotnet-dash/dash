// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
using Dash.Engine;
using Dash.Exceptions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Dash.Tests.Application
{
    public class DashApplicationTests
    {
        private readonly MockFileSystem _mockFileSystem = new MockFileSystem();
        private readonly ISourceCodeParser _sourceCodeParser = Substitute.For<ISourceCodeParser>();
        private readonly IConsole _console = Substitute.For<IConsole>();
        private readonly IDashOptionsValidator _dashOptionsValidator = Substitute.For<IDashOptionsValidator>();
        private readonly ISourceCodeProcessor _sourceCodeProcessor = Substitute.For<ISourceCodeProcessor>();


        [Fact]
        public async Task Run_ValidationFailed_ShouldNotParseSourceCode()
        {
            // Arrange
            var sut = ArrangeSut(new DashOptions());

            _dashOptionsValidator.Validate().Returns(false);

            // Act
            await sut.Run();

            // Assert
            _sourceCodeParser.DidNotReceive().Parse(Arg.Any<string>());
        }

        [Fact]
        public async Task Run_ValidationSucceeded_ShouldNotParseSourceCode()
        {
            // Arrange
            var sut = ArrangeSut(new DashOptions
            {
                InputFile = @"c:\test.json"
            });

            _mockFileSystem.AddFile(@"c:\test.json", new MockFileData("{}"));

            _dashOptionsValidator.Validate().Returns(true);

            // Act
            await sut.Run();

            // Assert
            _sourceCodeParser.Received(1).Parse("{}");
        }

        [Fact]
        public async Task Run_ParserExceptionThrown_ShouldWriteErrorToConsole()
        {
            // Arrange
            var sut = ArrangeSut(new DashOptions
            {
                InputFile = @"c:\test.json"
            });

            _mockFileSystem.AddFile(@"c:\test.json", new MockFileData("{}"));

            _dashOptionsValidator.Validate().Returns(true);

            _sourceCodeParser.Parse("{}").Throws(new ParserException("Foo"));

            // Act
            await sut.Run();

            // Assert
            _console.Error("Error while parsing the source code: Foo");
        }

        private DashApplication ArrangeSut(DashOptions dashOptions)
        {
            return new DashApplication(
                _console,
                _dashOptionsValidator,
                new OptionsWrapper<DashOptions>(dashOptions),
                _mockFileSystem,
                _sourceCodeParser,
                _sourceCodeProcessor);
        }
    }
}