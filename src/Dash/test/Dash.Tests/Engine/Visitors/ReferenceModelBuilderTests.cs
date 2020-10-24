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
using FluentArrange;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Arrange = FluentArrange.NSubstitute.Arrange;

namespace Dash.Tests.Engine.Visitors
{
    public class ReferenceModelBuilderTests
    {
        [Theory]
        [InlineData("Language", false)]
        [InlineData("Language?", true)]
        public async Task Visit_HasReferenceDeclarationNode_ShouldHaveUpdatedEntities(string referencedEntity, bool expectedIsNullable)
        {
            // Arrange
            var _context = Arrange.For<ReferenceModelBuilder>()
                .WithDependency<IEntityReferenceValueParser>(new EntityReferenceValueParser())
                .WithDependency<IModelRepository>(new DefaultModelRepository(), repository =>
                {
                    repository.Add(new EntityModel("Account"));
                });

            var account = new EntityDeclarationNode(new ModelNode(), "Account");
            await _context.Sut.Visit(account);

            var node = new HasReferenceDeclarationNode(
                account,
                "PreferredLanguage",
                referencedEntity);

            // Act
            await _context.Sut.Visit(node);

            // Assert
            _context.Dependency<IModelRepository>().EntityModels.Should().SatisfyRespectively(
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
            var _context = Arrange.For<ReferenceModelBuilder>()
                .WithDependency<IEntityReferenceValueParser>(new EntityReferenceValueParser())
                .WithDependency<IModelRepository>(new DefaultModelRepository(), repository =>
                {
                    repository.CreateEntityModel("Order", "OrderLine");
                });

            // TODO: the following should happen inside Arrange.For (FluentArrange needs to be modified)
            var modelNode = new ModelNode();
            var accountNode = modelNode.AddEntityDeclarationNode("Order");
            var orderLineNode = modelNode.AddEntityDeclarationNode("OrderLine");

            await _context.Sut.Visit(accountNode);
            await _context.Sut.Visit(orderLineNode);

            var node = new HasManyReferenceDeclarationNode(accountNode, "OrderLines", "OrderLine");

            // Act
            await _context.Sut.Visit(node);

            // Assert
            _context.Dependency<IModelRepository>().EntityModels.Should().SatisfyRespectively(
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
            var _context = Arrange.For<ReferenceModelBuilder>()
                .WithDependency<IEntityReferenceValueParser>(new EntityReferenceValueParser())
                .WithDependency<IModelRepository>(new DefaultModelRepository(), repository =>
                {
                    repository.CreateEntityModel("Order", "Product");
                });

            var modelNode = new ModelNode();
            var orderNode = modelNode.AddEntityDeclarationNode("Order");
            var productNode = modelNode.AddEntityDeclarationNode("Product");

            await _context.Sut.Visit(orderNode);
            await _context.Sut.Visit(productNode);

            var node = new HasAndBelongsToManyDeclarationNode(orderNode, "Product", "Product");

            // Act
            await _context.Sut.Visit(node);

            // Assert
            _context.Dependency<IModelRepository>().EntityModels.Should().SatisfyRespectively(
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
            var _context = Arrange.For<ReferenceModelBuilder>()
                .WithDependency<IEntityReferenceValueParser>(new EntityReferenceValueParser())
                .WithDependency<IModelRepository>(new DefaultModelRepository(), repository =>
                {
                    var foo = new EntityModel("Foo");
                    var bar = new EntityModel("Bar")
                        .WithAttribute<IntDataType>("Id", "Int", result =>
                        {
                            result.WithIsNullable(true).WithDefaultValue("123");
                        });

                    repository.Add(foo);
                    repository.Add(bar);
                });

            var node = new ModelNode()
                .AddEntityDeclarationNode("Foo")
                .AddInheritanceDeclaration("Bar");

            // Act
            await _context.Sut.Visit(node);

            // Assert
            _context.Dependency<IModelRepository>()
                .Get("Foo")
                .CodeAttributes.Should().SatisfyRespectively(
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
