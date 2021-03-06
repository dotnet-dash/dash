﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dash.Engine;
using Dash.Engine.Repositories;
using Dash.Engine.Visitors;
using Dash.Exceptions;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;
using Arrange = FluentArrange.NSubstitute.Arrange;

namespace Dash.Tests.Engine.Visitors
{
    public class DefaultSemanticAnalyzerTests
    {
        [Fact]
        public async Task Visit_UriNode_SingleDot_HasErrorsShouldBeFalse()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository());

            var uriNode = UriNode.ForExistingFile(new Uri(".", UriKind.Relative));

            // Act
            await context.Sut.Visit(uriNode);

            // Assert
            context.Dependency<IErrorRepository>().HasErrors().Should().BeFalse();
        }

        [Fact]
        public async Task Visit_UriNode_ContainsUnsupportedScheme_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository());

            var uriNode = UriNode.ForExistingFile(new Uri("https://foo"));

            // Act
            await context.Sut.Visit(uriNode);

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Unsupported scheme 'https' found in Uri 'https://foo/'. Supported schemes: file");
                });
        }

        [Fact]
        public async Task Visit_ModelNode_ShouldHaveVisitedEntityDeclarationNodes()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>();

            var modelNode = new ModelNode();
            modelNode.EntityDeclarations.Add(Substitute.For<EntityDeclarationNode>(modelNode, "Account"));
            modelNode.EntityDeclarations.Add(Substitute.For<EntityDeclarationNode>(modelNode, "Person"));

            // Act
            await context.Sut.Visit(modelNode);

            // Assert
            await modelNode.EntityDeclarations[0].Received(1).Accept(context.Sut);
            await modelNode.EntityDeclarations[1].Received(1).Accept(context.Sut);
        }

        [Fact]
        public async Task Visit_ModelNode_ContainsDuplicateEntityName_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository());

            var modelNode = new ModelNode();
            modelNode.AddEntityDeclarationNode("Account");
            modelNode.AddEntityDeclarationNode("Account");

            // Act
            await context.Sut.Visit(modelNode);

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Model contains duplicate declarations for entity 'Account'");
                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task Visit_EntityDeclarationNode_WithNullOrWhitespaceName_ShouldHaveUpdatedErrorRepository(string name)
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository());

            var entityDeclarationNode = new EntityDeclarationNode(new ModelNode(), name);

            // Act
            await context.Sut.Visit(entityDeclarationNode);

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Entity name cannot be null, empty or contain only white-spaces");
                });
        }

        [Theory]
        [InlineData("1")]
        [InlineData("/")]
        [InlineData("%%")]
        [InlineData("1ab")]
        [InlineData("ab1$")]
        public async Task Visit_EntityDeclarationNode_WithNonAllowedCharacters_ShouldHaveUpdatedErrorRepository(string name)
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository());

            var entityDeclarationNode = new EntityDeclarationNode(new ModelNode(), name);

            // Act
            await context.Sut.Visit(entityDeclarationNode);

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be($"'{name}' is an invalid name. You can only use alphanumeric characters, and it cannot start with a number");
                });
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_ContainsDuplicateAttributeNames_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository());

            var entityDeclarationNode = new EntityDeclarationNode(new ModelNode(), "Account");
            entityDeclarationNode.AddAttributeDeclaration("Id", "Int");
            entityDeclarationNode.AddAttributeDeclaration("Id", "Guid");

            // Act
            await context.Sut.Visit(entityDeclarationNode);

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Entity 'Account' contains duplicate declarations for attribute 'Id'");
                });
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_InheritedEntityNotFound_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository())
                .WithDependency<ISymbolRepository>(repository => repository.EntityExists("User").Returns(false));

            var node = new EntityDeclarationNode(new ModelNode(), "Account");
            node.AddInheritanceDeclaration("User");

            // Act
            await context.Sut.Visit(node);

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Entity 'Account' wants to inherit unknown entity 'User'");
                });
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_MultipleInheritanceDeclaration_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository())
                .WithDependency<ISymbolRepository>(repository => repository.EntityExists(Arg.Any<string>()).Returns(true));

            var node = new EntityDeclarationNode(new ModelNode(), "FooBar");
            node.AddInheritanceDeclaration("Foo");
            node.AddInheritanceDeclaration("Bar");

            // Act
            await context.Sut.Visit(node);

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
            first =>
            {
                first.Should().Be("Multiple inheritance declaration found for 'FooBar'");
            });
        }

        [Fact]
        public async Task Visit_AttributeDeclarationNode_ThrowsInvalidDataTypeException_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository())
                .WithDependency<IDataTypeDeclarationParser>(parser => parser.Parse("Invalid").Throws(e => new InvalidDataTypeException("Invalid")));

            var entity = new EntityDeclarationNode(new ModelNode(), "Foo");
            var node = new AttributeDeclarationNode(entity, "Id", "Invalid");

            // Act
            await context.Sut.Visit(node);

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be("The specified datatype 'Invalid' is invalid"));
        }

        [Fact]
        public async Task Visit_AttributeDeclarationNode_ThrowsInvalidDataTypeConstraintException_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository())
                .WithDependency<IDataTypeDeclarationParser>(parser => parser.Parse("Invalid").Throws(e => new InvalidDataTypeConstraintException("Invalid data type")));

            var entity = new EntityDeclarationNode(new ModelNode(), "Foo");
            var node = new AttributeDeclarationNode(entity, "Id", "Invalid");

            // Act
            await context.Sut.Visit(node);

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Invalid data type");
                });
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_SelfInheritance_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository())
                .WithDependency<ISymbolRepository>(repository => repository.EntityExists("Foo").Returns(true));

            var node = new EntityDeclarationNode(new ModelNode(), "Foo");
            node.AddInheritanceDeclaration("Foo");

            await context.Sut.Visit(node);

            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be("Self-inheritance not allowed: 'Foo'"));
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_ReservedEntityNameUsed_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository())
                .WithDependency<IReservedSymbolProvider>(provider => provider.IsReservedEntityName("Foo").Returns(true));

            // Act
            await context.Sut.Visit(new EntityDeclarationNode(new ModelNode(), "Foo"));

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be("'Foo' is a reserved name and cannot be used as an entity name."));
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_MultipleAbstractionDeclarationNodesFound_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository());

            var entityDeclarationNode = new EntityDeclarationNode(new ModelNode(), "Foo")
                .AddAbstractDeclarationNode(true)
                .AddAbstractDeclarationNode(false);

            // Act
            await context.Sut.Visit(entityDeclarationNode);

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be("Multiple abstract declarations found for 'Foo'"));
        }

        [Fact]
        public async Task Visit_CsvSeedDeclarationNode_HeaderNameIsNotAnAttribute_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var context = Arrange.For<DefaultSemanticAnalyzer>()
                .WithDependency<IErrorRepository>(new ErrorRepository());

            var node = new CsvSeedDeclarationNode(
                new EntityDeclarationNode(new ModelNode(), "Foo"),
                new Uri("https://unittest"),
                false,
                null,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "NonExistingAttribute", "CsvHeader" }
                });

            // Act
            await context.Sut.Visit(node);

            // Assert
            context.Dependency<IErrorRepository>().GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be("Trying to map header 'CsvHeader' to unknown Entity Attribute 'NonExistingAttribute'"));
        }
    }
}