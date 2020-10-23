// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Common;
using Dash.Engine;
using Dash.Engine.DataTypes;
using Dash.Engine.Models;
using Dash.Engine.Parsers;
using Dash.Engine.Parsers.Result;
using Dash.Engine.Repositories;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Visitors
{
    public class ReferenceModelBuilderTests
    {
        private readonly IModelRepository _modelRepository;
        private readonly ReferenceModelBuilder _sut;

        public ReferenceModelBuilderTests()
        {
            _modelRepository = new DefaultModelRepository();
            _sut = new ReferenceModelBuilder(_modelRepository, new EntityReferenceValueParser(), Substitute.For<IConsole>());
        }

        [Theory]
        [InlineData("Language", false)]
        [InlineData("Language?", true)]
        public async Task Visit_HasReferenceDeclarationNode_ShouldHaveUpdatedEntities(string referencedEntity, bool expectedIsNullable)
        {
            // Arrange
            _modelRepository.Add(new EntityModel("Account"));

            var account = new EntityDeclarationNode(new ModelNode(), "Account");
            await _sut.Visit(account);

            var node = new HasReferenceDeclarationNode(
                account,
                "PreferredLanguage",
                referencedEntity);

            // Act
            await _sut.Visit(node);

            // Assert
            _modelRepository.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.SingleReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("PreferredLanguage");
                            a.EntityModel.Should().Be("Language");
                            a.IsNullable.Should().Be(expectedIsNullable);
                        });
                });
        }

        [Fact]
        public async Task Visit_HasManyReferenceDeclarationNode_ShouldHaveUpdatedEntities()
        {
            // Arrange
            _modelRepository.CreateEntityModel("Order", "OrderLine");

            var modelNode = new ModelNode();
            var accountNode = modelNode.AddEntityDeclarationNode("Order");
            var orderLineNode = modelNode.AddEntityDeclarationNode("OrderLine");

            await _sut.Visit(accountNode);
            await _sut.Visit(orderLineNode);

            var node = new HasManyReferenceDeclarationNode(accountNode, "OrderLines", "OrderLine");

            // Act
            await _sut.Visit(node);

            // Assert
            _modelRepository.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Order");
                    first.CollectionReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("OrderLines");
                            a.EntityModel.Should().Be("OrderLine");
                        });
                },
                second =>
                {
                    second.Name.Should().Be("OrderLine");
                    second.SingleReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("Order");
                            a.EntityModel.Should().Be("Order");
                        });
                });
        }

        [Fact]
        public async Task Visit_HasAndBelongsToManyDeclarationNode_ShouldHaveUpdatedEntities()
        {
            // Arrange
            _modelRepository.CreateEntityModel("Order", "Product");

            var modelNode = new ModelNode();
            var orderNode = modelNode.AddEntityDeclarationNode("Order");
            var productNode = modelNode.AddEntityDeclarationNode("Product");

            await _sut.Visit(orderNode);
            await _sut.Visit(productNode);

            var node = new HasAndBelongsToManyDeclarationNode(orderNode, "Product", "Product");

            // Act
            await _sut.Visit(node);

            // Assert
            _modelRepository.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Order");
                    first.CollectionReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("OrderProduct");
                            a.EntityModel.Should().Be("OrderProduct");
                            a.IsNullable.Should().BeFalse();
                        });
                },
                second =>
                {
                    second.Name.Should().Be("Product");
                    second.CollectionReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("OrderProduct");
                            a.EntityModel.Should().Be("OrderProduct");
                            a.IsNullable.Should().BeFalse();
                        });
                });
        }

        [Fact]
        public async Task Visit_InheritanceDeclarationNode_ShouldHaveCalledMethodInheritAttributes()
        {
            // Arrange
            var foo = new EntityModel("Foo");
            var bar = new EntityModel("Bar")
                .WithAttribute<IntDataType>("Id", "Int", result =>
                {
                    result.WithIsNullable(true).WithDefaultValue("123");
                });

            _modelRepository.Add(foo);
            _modelRepository.Add(bar);

            InheritanceDeclarationNode node = new ModelNode()
                .AddEntityDeclarationNode("Foo")
                .AddInheritanceDeclaration("Bar");

            // Act
            await _sut.Visit(node);

            // Assert
            foo.CodeAttributes.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Id");
                    first.TargetEnvironmentDataType.Should().Be("Int");
                    first.IsNullable.Should().BeTrue();
                    first.DefaultValue.Should().Be("123");
                });
        }
    }
}
