﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
using Dash.Engine;
using Dash.Engine.Generator;
using Dash.Nodes;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Generator
{
    public class DefaultGeneratorTests
    {
        [Fact]
        public async Task Generate_HappyFlow_ShouldHaveWrittenFileToFileSystem()
        {
            // Arrange
            var uriResourceRepository = Substitute.For<IUriResourceRepository>();
            uriResourceRepository.GetContents(new Uri("dash://poco")).Returns("poco template");
            uriResourceRepository.GetContents(new Uri("dash://poco2")).Returns("poco template 2");

            var fileSystem = new MockFileSystem();

            var modelRepository = Substitute.For<IModelRepository>();

            var sut = new DefaultGenerator(
                uriResourceRepository,
                fileSystem,
                modelRepository,
                Substitute.For<IConsole>(),
                new OptionsWrapper<DashOptions>(new DashOptions()));

            var configuration = new ConfigurationNode()
                .AddTemplateNode("dash://poco", @"c:\temp\")
                .AddTemplateNode("dash://poco2", @"c:\temp2\sub\");

            var sourceCodeDocument = new SourceCodeNode(configuration, new ModelNode());

            // Act
            await sut.Generate(sourceCodeDocument);

            // Assert
            fileSystem.GetFile(@"c:\temp\poco.generated.cs").TextContents.Should().Be("poco template");
            fileSystem.GetFile(@"c:\temp2\sub\poco2.generated.cs").TextContents.Should().Be("poco template 2");
        }
    }
}