// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
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
        private readonly Program _sut;

        public ProgramTests()
        {
            _mockFileSystem = new MockFileSystem();
            _console = Substitute.For<IConsole>();

            var serviceProvider = new ApplicationServiceProvider();
            var services = serviceProvider.CreateServiceCollection(false, ".");
            services.Replace(new ServiceDescriptor(typeof(IFileSystem), _mockFileSystem));
            services.Replace(new ServiceDescriptor(typeof(IConsole), _console));

            var mockedServiceProvider = Substitute.For<IApplicationServiceProvider>();
            mockedServiceProvider.CreateServiceCollection(false, ".").Returns(services);

            _sut = new Program(mockedServiceProvider);
        }

        [Fact]
        public async Task Run_FileIsNull_ShouldHavePrintedErrorMessage()
        {
            await _sut.Run(null, ".", false);

            // Assert
            _console.Received(1).Error("Please specify a model file.");
        }

        [Fact]
        public async Task MisconfigurationNoTemplateDefined()
        {
            // Arrange
            ArrangeFile("Errors/MisconfigurationNoTemplateDefined.json");

            // Act
            await _sut.Run("c:/temp/sut.json", ".", false);

            // Assert
            _console.Received(1).Error("Configuration.Templates[0] object has no 'Template' property.");
        }

        [Fact]
        public async Task EmbeddedTemplateNotFound()
        {
            // Arrange
            ArrangeFile("Errors/EmbeddedTemplateNotFound.json");

            // Act
            await _sut.Run("c:/temp/sut.json", ".", false);

            // Assert
            _console.Received(1).Error("Dash template does not exist: dash://unknown/");
        }

        private void ArrangeFile(string fileName)
        {
            _mockFileSystem.AddFile("c:/temp/sut.json", new MockFileData(File.ReadAllText($"Samples/{fileName}")));
        }
    }
}
