﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine;
using Dash.Engine.LanguageProviders;
using Dash.Engine.Parsers;
using Dash.Engine.Repositories;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class DefaultModelBuilderTests
    {
        private readonly IModelRepository _modelRepository;
        private readonly DefaultModelBuilder _sut;

        public DefaultModelBuilderTests()
        {
            _modelRepository = new DefaultModelRepository();
            _sut = new DefaultModelBuilder(
                new DataTypeDeclarationParser(),
                new List<ILanguageProvider>
                {
                    new CSharpLanguageProvider(),
                    new SqlServerLanguageProvider()
                },
                _modelRepository,
                Substitute.For<IConsole>());
        }

        [Fact]
        public async Task Visit_ModelNode_EntityModelCreated()
        {
            // Arrange
            var modelNode = new ModelNode();

            modelNode
                .AddEntityDeclarationNode("Country")
                .AddAttributeDeclaration("Id", "Int");

            modelNode
                .AddEntityDeclarationNode("Person")
                .AddHasDeclaration("CountryOfBirth", "Country")
                .AddHasDeclaration("CountryOfResidence", "Country");

            // Act
            await _sut.Visit(modelNode);

            // Assert
            _modelRepository.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Country");
                    first.SingleReferences.Should().BeEmpty();
                    first.CollectionReferences.Should().BeEmpty();
                },
                second =>
                {
                    second.Name.Should().Be("Person");
                    second.SingleReferences.Should().BeEmpty();
                    second.CollectionReferences.Should().BeEmpty();
                });
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_EntityModelCreated()
        {
            // Arrange
            var modelNode = new ModelNode();
            var node = modelNode
                .AddEntityDeclarationNode("Account")
                .AddAttributeDeclaration("Surname", "Unicode")
                .AddAttributeDeclaration("Username", "String")
                .AddAttributeDeclaration("Nickname", "Unicode?");

            // Act
            await _sut.Visit(node);

            // Assert
            _modelRepository.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Account");
                    first.CodeAttributes.Should().SatisfyRespectively(
                        a =>
                        {
                            a.Name.Should().Be("Surname");
                            a.TargetEnvironmentDataType.Should().Be("string");
                            a.IsNullable.Should().BeFalse();
                        },
                        b =>
                        {
                            b.Name.Should().Be("Username");
                            b.TargetEnvironmentDataType.Should().Be("string");
                            b.IsNullable.Should().BeFalse();
                        },
                        c =>
                        {
                            c.Name.Should().Be("Nickname");
                            c.TargetEnvironmentDataType.Should().Be("string");
                            c.IsNullable.Should().BeTrue();
                        });

                    first.DataAttributes.Should().SatisfyRespectively(
                        a =>
                        {
                            a.Name.Should().Be("Surname");
                            a.TargetEnvironmentDataType.Should().Be("nvarchar");
                            a.IsNullable.Should().BeFalse();
                        },
                        b =>
                        {
                            b.Name.Should().Be("Username");
                            b.TargetEnvironmentDataType.Should().Be("varchar");
                            b.IsNullable.Should().BeFalse();
                        },
                        c =>
                        {
                            c.Name.Should().Be("Nickname");
                            c.TargetEnvironmentDataType.Should().Be("nvarchar");
                            c.IsNullable.Should().BeTrue();
                        });
                });
        }
    }
}
