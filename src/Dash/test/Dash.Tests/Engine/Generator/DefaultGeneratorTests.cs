// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
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
        [Fact]
        public async Task Generate_HappyFlow_ShouldHaveWrittenFileToFileSystem()
        {
            // Arrange
            var uriResourceRepository = Substitute.For<IUriResourceRepository>();
            uriResourceRepository.GetContents(new Uri("dash://foo")).Returns("foo template");
            uriResourceRepository.GetContents(new Uri("dash://bar")).Returns("bar template");

            var buildOutputRepository = Substitute.For<IBuildOutputRepository>();

            var templateTransformer = Substitute.For<ITemplateTransformer>();
            templateTransformer.Transform("foo template").Returns("foo template transformed");
            templateTransformer.Transform("bar template").Returns("bar template transformed");

            var sut = new DefaultGenerator(
                uriResourceRepository,
                Substitute.For<IConsole>(),
                buildOutputRepository,
                templateTransformer,
                new OptionsWrapper<DashOptions>(new DashOptions()));

            var configuration = new ConfigurationNode()
                .AddTemplateNode("dash://foo", @"c:\foo\")
                .AddTemplateNode("dash://bar", @"c:\bar\sub\");

            var sourceCodeDocument = new SourceCodeNode(configuration, new ModelNode());

            // Act
            await sut.Generate(sourceCodeDocument);

            // Assert
            buildOutputRepository.Received(1).Add(@"c:/foo/foo.generated.cs", "foo template transformed");
            buildOutputRepository.Received(1).Add(@"c:/bar/sub/bar.generated.cs", "bar template transformed");
        }
    }
}