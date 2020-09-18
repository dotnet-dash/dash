// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
using Dash.Engine;
using Dash.Engine.Generator;
using Dash.Nodes;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Generator
{
    public class DefaultGeneratorTests
    {
        private readonly IUriResourceRepository _uriResourceRepository = Substitute.For<IUriResourceRepository>();
        private readonly IBuildOutputRepository _buildOutputRepository = Substitute.For<IBuildOutputRepository>();
        private readonly ITemplateTransformer _templateTransformer = Substitute.For<ITemplateTransformer>();
        private readonly IFileSystem _mockFileSystem = new MockFileSystem();
        private readonly DefaultGenerator _sut;

        public DefaultGeneratorTests()
        {
            _sut = new DefaultGenerator(
                _uriResourceRepository,
                Substitute.For<IConsole>(),
                _buildOutputRepository,
                _templateTransformer,
                new OptionsWrapper<DashOptions>(new DashOptions()),
                _mockFileSystem);
        }

        [Fact]
        public async Task Generate_HappyFlow_ShouldHaveWrittenFileToFileSystem()
        {
            // Arrange
            _uriResourceRepository.GetContents(new Uri("dash://foo")).Returns("foo template");
            _uriResourceRepository.GetContents(new Uri("dash://bar")).Returns("bar template");

            _templateTransformer.Transform("foo template").Returns("foo template transformed");
            _templateTransformer.Transform("bar template").Returns("bar template transformed");

            var configuration = new ConfigurationNode()
                .AddTemplateNode("dash://foo", @"c:\foo\")
                .AddTemplateNode("dash://bar", @"c:\bar\sub\");

            configuration.Header = null;

            var sourceCodeDocument = new SourceCodeNode(configuration, new ModelNode());

            // Act
            await _sut.Generate(sourceCodeDocument);

            // Assert
            _buildOutputRepository.Received(1).Add(@"c:/foo/foo.generated.cs", "foo template transformed");
            _buildOutputRepository.Received(1).Add(@"c:/bar/sub/bar.generated.cs", "bar template transformed");
        }

        [Fact]
        public async Task Generate_RelativePaths_ShouldWriteTo()
        {
            // Arrange
            _uriResourceRepository.GetContents(new Uri("dash://foo")).Returns("foo template");
            _templateTransformer.Transform("foo template").Returns("foo template transformed");
            _mockFileSystem.Directory.SetCurrentDirectory("c:/output/");

            var configuration = new ConfigurationNode()
                .AddTemplateNode("dash://foo", ".");
            configuration.Header = null;

            var sourceCodeDocument = new SourceCodeNode(configuration, new ModelNode());

            // Act
            await _sut.Generate(sourceCodeDocument);

            // Assert
            _buildOutputRepository.Received(1).Add(Arg.Any<string>(), Arg.Any<string>());
            _buildOutputRepository.Received(1).Add(@"c:/output/foo.generated.cs", "foo template transformed");
        }

        [Fact]
        public async Task Generate_HeaderDefined_ShouldUseDefinedHeader()
        {
            // Arrange
            _uriResourceRepository.GetContents(new Uri("dash://foo")).Returns("foo template");
            _templateTransformer.Transform("foo template").Returns("foo template transformed");
            _mockFileSystem.Directory.SetCurrentDirectory("c:/output/");

            var configuration = new ConfigurationNode()
                .AddTemplateNode("dash://foo", ".");
            configuration.Header = "Header";

            var sourceCodeDocument = new SourceCodeNode(configuration, new ModelNode());

            // Act
            await _sut.Generate(sourceCodeDocument);

            // Assert
            _buildOutputRepository.Received(1).Add(Arg.Any<string>(), Arg.Any<string>());
            _buildOutputRepository.Received(1).Add(@"c:/output/foo.generated.cs", "// Header" + Environment.NewLine + Environment.NewLine + "foo template transformed");
        }

        [Theory]
        [InlineData("autogen")]
        [InlineData("autogen.")]
        [InlineData(".autogen")]
        [InlineData("..autogen..")]
        public async Task Generate_AutogenSuffixDefined_ShouldUseAutogenSuffix(string autogenSuffix)
        {
            // Arrange
            _uriResourceRepository.GetContents(new Uri("dash://foo")).Returns("foo template");
            _templateTransformer.Transform("foo template").Returns("foo template transformed");
            _mockFileSystem.Directory.SetCurrentDirectory("c:/output/");

            var configuration = new ConfigurationNode()
                .AddTemplateNode("dash://foo", ".");
            configuration.Header = null;
            configuration.AutogenSuffix = autogenSuffix;

            var sourceCodeDocument = new SourceCodeNode(configuration, new ModelNode());

            // Act
            await _sut.Generate(sourceCodeDocument);

            // Assert
            _buildOutputRepository.Received(1).Add(Arg.Any<string>(), Arg.Any<string>());
            _buildOutputRepository.Received(1).Add(@"c:/output/foo.autogen.cs", "foo template transformed");

        }
    }
}