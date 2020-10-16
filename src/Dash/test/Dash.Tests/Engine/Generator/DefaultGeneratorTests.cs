// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Engine;
using Dash.Engine.Generator;
using Dash.Engine.Models;
using Dash.Nodes;
using FluentArrange.NSubstitute;
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
            var context = Arrange.For<DefaultGenerator>()
                .WithDependency<IUriResourceRepository>(d =>
                {
                    d.GetContents(new Uri("dash://foo")).Returns("foo template");
                    d.GetContents(new Uri("dash://bar")).Returns("bar template");
                })
                .WithDependency<ITemplateTransformer>(d =>
                {
                    d.Transform("foo template", Arg.Any<IEnumerable<EntityModel>>()).Returns("foo template transformed");
                    d.Transform("bar template", Arg.Any<IEnumerable<EntityModel>>()).Returns("bar template transformed");
                })
                .WithDependency<IOptions<DashOptions>>(new OptionsWrapper<DashOptions>(new DashOptions()))
                .WithDependency<IFileSystem>(new MockFileSystem());

            var configuration = new ConfigurationNode()
                .AddTemplateNode("dash://foo", @"c:\foo\")
                .AddTemplateNode("dash://bar", @"c:\bar\sub\")
                .WithNullHeader();

            var sourceCodeDocument = new SourceCodeNode(configuration, new ModelNode());

            // Act
            await context.Sut.Generate(sourceCodeDocument);

            // Assert
            context.Dependency<IBuildOutputRepository>().Received(1).Add(@"c:/foo/foo.generated.cs", "foo template transformed");
            context.Dependency<IBuildOutputRepository>().Received(1).Add(@"c:/bar/sub/bar.generated.cs", "bar template transformed");
        }

        [Fact]
        public async Task Generate_RelativePaths_ShouldWriteTo()
        {
            // Arrange
            var context = Arrange.For<DefaultGenerator>()
                .WithDependency<IUriResourceRepository>(d => d.GetContents(new Uri("dash://foo/")).Returns("foo template"))
                .WithDependency<ITemplateTransformer>(d => d.Transform("foo template", Arg.Any<IEnumerable<EntityModel>>()).Returns("foo template transformed"))
                .WithDependency<IFileSystem>(new MockFileSystem(), d => d.Directory.SetCurrentDirectory("c:/output/"))
                .WithDependency<IOptions<DashOptions>>(new OptionsWrapper<DashOptions>(new DashOptions()));

            var configuration = new ConfigurationNode()
                .AddTemplateNode("dash://foo", ".")
                .WithNullHeader();

            var sourceCodeDocument = new SourceCodeNode(configuration, new ModelNode());

            // Act
            await context.Sut.Generate(sourceCodeDocument);

            // Assert
            context.Dependency<IBuildOutputRepository>().Received(1).Add(Arg.Any<string>(), Arg.Any<string>());
            context.Dependency<IBuildOutputRepository>().Received(1).Add(@"c:/output/foo.generated.cs", "foo template transformed");
        }

        [Fact]
        public async Task Generate_HeaderDefined_ShouldUseDefinedHeader()
        {
            // Arrange
            var context = Arrange.For<DefaultGenerator>()
                .WithDependency<IUriResourceRepository>(d => d.GetContents(new Uri("dash://foo/")).Returns("foo template"))
                .WithDependency<ITemplateTransformer>(d => d.Transform("foo template", Arg.Any<IEnumerable<EntityModel>>()).Returns("foo template transformed"))
                .WithDependency<IFileSystem>(new MockFileSystem(), d => d.Directory.SetCurrentDirectory("c:/output/"))
                .WithDependency<IOptions<DashOptions>>(new OptionsWrapper<DashOptions>(new DashOptions()));

            var configuration = new ConfigurationNode()
                .AddTemplateNode("dash://foo", ".")
                .WithHeader("Header");

            var sourceCodeDocument = new SourceCodeNode(configuration, new ModelNode());

            // Act
            await context.Sut.Generate(sourceCodeDocument);

            // Assert
            context.Dependency<IBuildOutputRepository>().Received(1).Add(Arg.Any<string>(), Arg.Any<string>());
            context.Dependency<IBuildOutputRepository>().Received(1).Add(@"c:/output/foo.generated.cs", "// Header" + Environment.NewLine + Environment.NewLine + "foo template transformed");
        }

        [Theory]
        [InlineData("autogen")]
        [InlineData("autogen.")]
        [InlineData(".autogen")]
        [InlineData("..autogen..")]
        public async Task Generate_AutogenSuffixDefined_ShouldUseAutogenSuffix(string autogenSuffix)
        {
            // Arrange
            var context = Arrange.For<DefaultGenerator>()
                .WithDependency<IUriResourceRepository>(d => d.GetContents(new Uri("dash://foo")).Returns("foo template"))
                .WithDependency<IModelRepository>(d => d.EntityModels.Returns(new[] {new EntityModel("Foo")}))
                .WithDependency<ITemplateTransformer>(d => d.Transform("foo template", Arg.Is<IEnumerable<EntityModel>>(e => e.Single().Name == "Foo")).Returns("foo template transformed"))
                .WithDependency<IFileSystem>(new MockFileSystem(), d => d.Directory.SetCurrentDirectory("c:/output/"))
                .WithDependency<IOptions<DashOptions>>(new OptionsWrapper<DashOptions>(new DashOptions()));

            var configuration = new ConfigurationNode()
                .AddTemplateNode("dash://foo", ".")
                .WithNullHeader()
                .WithAutogenSuffix(autogenSuffix);

            var sourceCodeDocument = new SourceCodeNode(configuration, new ModelNode());

            // Act
            await context.Sut.Generate(sourceCodeDocument);

            // Assert
            context.Dependency<IBuildOutputRepository>().Received(1).Add(Arg.Any<string>(), Arg.Any<string>());
            context.Dependency<IBuildOutputRepository>().Received(1).Add(@"c:/output/foo.autogen.cs", "foo template transformed");
        }
    }
}