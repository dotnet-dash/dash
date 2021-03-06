﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Application.Default;
using Dash.Common;
using Dash.Engine.Generator;
using Dash.Roslyn;
using Dash.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Xunit;

namespace Dash.Tests
{
    public class ProgramTests
    {
        private readonly MockFileSystem _mockFileSystem;
        private readonly IConsole _console;
        private readonly IWorkspace _workspace;
        private readonly IProject _project;

        public ProgramTests()
        {
            _mockFileSystem = new MockFileSystem(null, "c:/project/");
            _mockFileSystem.AddFile("c:/project/project.csproj", new MockFileData(string.Empty));
            _console = Substitute.For<IConsole>();

            _project = Substitute.For<IProject>();

            _workspace = Substitute.For<IWorkspace>();
            _workspace.OpenProjectAsync().Returns(_project);
        }

        [Fact]
        public async Task Run_FileIsNull_ShouldHavePrintedErrorMessage()
        {
            // Arrange
            var options = new DashOptions();
            var sut = ArrangeSut(options);

            // Act
            await sut.Run(options);

            // Assert
            _console.Received(1).Error("Please specify a model file.");
        }

        [Fact]
        public async Task MisconfigurationNoTemplateDefined()
        {
            // Arrange
            ArrangeFile("Errors/MisconfigurationNoTemplateDefined.json");
            var options = new DashOptions
            {
                InputFile = "c:/project/sut.json",
                Project = null,
                Verbose = false,
            };

            var sut = ArrangeSut(options);

            // Act
            await sut.Run(options);

            // Assert
            _console.Received(1).Error("Configuration.Templates[0] object has no 'Template' property.");
        }

        [Fact]
        public async Task EmbeddedTemplateNotFound()
        {
            // Arrange
            ArrangeFile("Errors/EmbeddedTemplateNotFound.json");
            var options = new DashOptions
            {
                InputFile = "c:/project/sut.json",
                Project = null,
                Verbose = false,
            };

            var sut = ArrangeSut(options);

            // Act
            await sut.Run(options);

            // Assert
            _console.Received(1).Error("Dash template does not exist: dash://unknown/");
        }

        [Fact]
        public async Task IntegrationTest_HelloWorld()
        {
            // Arrange
            ArrangeFile("HelloWorld.json");
            var expectedOutput = GetExpectedFileOutput("HelloWorld");
            var options = new DashOptions
            {
                InputFile = "c:/project/sut.json",
            };

            _project.Namespace.Returns("Hello");

            var sut = ArrangeSut(options);

            // Act
            await sut.Run(options);

            // Assert
            var generatedCode = _mockFileSystem.File.ReadAllText("c:/project/efcontext.generated.cs");

            generatedCode.Should().HaveSameTree(expectedOutput);
        }

        [Fact]
        public async Task IntegrationTest_Blog()
        {
            // Arrange
            ArrangeFile("IntegrationTests/Blog.json");
            var expectedEfContextOutput = GetExpectedFileOutput("Blog_EfContext");
            var expectedEfPocoOutput = GetExpectedFileOutput("Blog_EfPoco");

            var options = new DashOptions
            {
                InputFile = "c:/project/sut.json",
            };

            _project.Namespace.Returns("Foo");

            var sut = ArrangeSut(options);

            // Act
            await sut.Run(options);

            // Assert
            _mockFileSystem.File.ReadAllText("c:/project/efcontext.generated.cs").Should().HaveSameTree(expectedEfContextOutput);
            _mockFileSystem.File.ReadAllText("c:/project/efpoco.generated.cs").Should().HaveSameTree(expectedEfPocoOutput);
        }

        private Program ArrangeSut(DashOptions options)
        {
            var startup = new Startup();
            var services = startup.ConfigureServices(options);
            services.Replace(new ServiceDescriptor(typeof(IFileSystem), _mockFileSystem));
            services.Replace(new ServiceDescriptor(typeof(IConsole), _console));
            services.Replace(new ServiceDescriptor(typeof(IWorkspace), _workspace));
            services.Remove(services.First(e => e.ImplementationType == typeof(EditorConfigCodeFormatter)));

            var mockedStartup = Substitute.For<IStartup>();
            mockedStartup.ConfigureServices(options).Returns(services);

            return new Program(mockedStartup);
        }
        private void ArrangeFile(string fileName)
        {
            _mockFileSystem.AddFile("c:/project/sut.json", new MockFileData(File.ReadAllText($"Samples/{fileName}")));
        }

        private string GetExpectedFileOutput(string fileName)
        {
            return File.ReadAllText($"Samples/ExpectedOutput/{fileName}");
        }
    }
}
