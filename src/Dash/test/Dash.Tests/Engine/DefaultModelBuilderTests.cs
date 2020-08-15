using System.Collections.Generic;
using System.Linq;
using Dash.Engine;
using Dash.Engine.Abstractions;
using Dash.Engine.JsonParser;
using Dash.Engine.LanguageProviders;
using Dash.Nodes;
using FluentAssertions;
using FluentAssertions.Common;
using Xunit;

namespace Dash.Tests.Engine
{
    public class DefaultModelBuilderTests
    {
        [Fact]
        public void Visit_EntityDeclarationNode_EntityModelCreated()
        {
            // Arrange
            var sut = new DefaultModelBuilder(
                new DataTypeParser(),
                new EntityReferenceValueParser(),
                new List<ILanguageProvider>
            {
                new CSharpLanguageProvider(),
                new SqlServerLanguageProvider()
            });

            var node = new EntityDeclarationNode("Account");
            node.AddAttributeDeclaration("Surname", "Unicode");
            node.AddAttributeDeclaration("Username", "String");
            node.AddAttributeDeclaration("Nickname", "Unicode?");

            // Act
            sut.Visit(node);

            // Assert
            sut.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Account");
                    first.CodeAttributeModels.Should().SatisfyRespectively(
                        a =>
                        {
                            a.Name.Should().Be("Surname");
                            a.DataType.Should().Be("string");
                            a.IsNullable.Should().BeFalse();
                        },
                        b =>
                        {
                            b.Name.Should().Be("Username");
                            b.DataType.Should().Be("string");
                            b.IsNullable.Should().BeFalse();
                        },
                        c =>
                        {
                            c.Name.Should().Be("Nickname");
                            c.DataType.Should().Be("string");
                            c.IsNullable.Should().BeTrue();
                        });

                    first.DataAttributeModels.Should().SatisfyRespectively(
                        a =>
                        {
                            a.Name.Should().Be("Surname");
                            a.DataType.Should().Be("nvarchar");
                            a.IsNullable.Should().BeFalse();
                        },
                        b =>
                        {
                            b.Name.Should().Be("Username");
                            b.DataType.Should().Be("varchar");
                            b.IsNullable.Should().BeFalse();
                        },
                        c =>
                        {
                            c.Name.Should().Be("Nickname");
                            c.DataType.Should().Be("nvarchar");
                            c.IsNullable.Should().BeTrue();
                        });
                });
        }

        [Fact]
        public void Visit_ModelNode_EntityModelCreated()
        {
            // Arrange
            var sut = new DefaultModelBuilder(
                new DataTypeParser(),
                new EntityReferenceValueParser(),
                new List<ILanguageProvider>
            {
                new CSharpLanguageProvider(),
                new SqlServerLanguageProvider()
            });

            var countryNode = new EntityDeclarationNode("Country");
            countryNode.AddAttributeDeclaration("Id", "Int");

            var personNode = new EntityDeclarationNode("Person");
            personNode.AddHasDeclaration("CountryOfBirth", "Country");
            personNode.AddHasDeclaration("CountryOfResidence", "Country");

            var modelNode = new ModelNode
            {
                EntityDeclarations = {countryNode, personNode}
            };

            // Act
            sut.Visit(modelNode);

            // Assert
            sut.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Country");
                },
                second =>
                {
                    second.Name.Should().Be("Person");
                    second.SingleReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("CountryOfBirth");
                        },
                        b =>
                        {
                            b.ReferenceName.Should().Be("CountryOfResidence");
                        });
                });
        }

        [Theory]
        [InlineData("Language", false)]
        [InlineData("Language?", true)]
        public void Visit_HasReferenceDeclarationNode_ShouldHaveUpdatedEntities(string referencedEntity, bool expectedIsNullable)
        {
            // Arrange
            var sut = new DefaultModelBuilder(
                new DataTypeParser(),
                new EntityReferenceValueParser(),
                new List<ILanguageProvider>
                {
                    new CSharpLanguageProvider(),
                    new SqlServerLanguageProvider()
                });

            var account = new EntityDeclarationNode("Account");

            sut.Visit(account);

            var node = new HasReferenceDeclarationNode(
                account,
                "PreferredLanguage",
                referencedEntity);

            // Act
            sut.Visit(node);

            // Assert
            sut.EntityModels.Should().SatisfyRespectively(
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
        public void Visit_HasManyReferenceDeclarationNode_ShouldHaveUpdatedEntities()
        {
            // Arrange
            var sut = new DefaultModelBuilder(
                new DataTypeParser(),
                new EntityReferenceValueParser(),
                new List<ILanguageProvider>
                {
                    new CSharpLanguageProvider(),
                    new SqlServerLanguageProvider()
                });

            var account = new EntityDeclarationNode("Order");
            sut.Visit(account);
            sut.Visit(new EntityDeclarationNode("OrderLine"));

            var node = new HasManyReferenceDeclarationNode(account, "OrderLines", "OrderLine");

            // Act
            sut.Visit(node);

            // Assert
            sut.EntityModels.Should().SatisfyRespectively(
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
        public void Visit_HasAndBelongsToManyDeclarationNode_ShouldHaveUpdatedEntities()
        {
            // Arrange
            var sut = new DefaultModelBuilder(
                new DataTypeParser(),
                new EntityReferenceValueParser(),
                new List<ILanguageProvider>
                {
                    new CSharpLanguageProvider(),
                    new SqlServerLanguageProvider()
                });

            sut.Visit(new EntityDeclarationNode("Student"));
            sut.Visit(new EntityDeclarationNode("Course"));

            var node = new HasAndBelongsToManyDeclarationNode(new EntityDeclarationNode("Student"), "Course", "Course");

            // Act
            sut.Visit(node);

            // Assert
            sut.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Student");
                    first.CollectionReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("StudentCourse");
                            a.EntityModel.Should().Be("StudentCourse");
                            a.IsNullable.Should().BeFalse();
                        });
                },
                second =>
                {
                    second.Name.Should().Be("Course");
                    second.CollectionReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("StudentCourse");
                            a.EntityModel.Should().Be("StudentCourse");
                            a.IsNullable.Should().BeFalse();
                        });
                },
                third =>
                {
                    third.Name.Should().Be("StudentCourse");
                    third.SingleReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("Student");
                            a.EntityModel.Should().Be("Student");
                            a.IsNullable.Should().BeFalse();
                        },
                        b =>
                        {
                            b.ReferenceName.Should().Be("Course");
                            b.EntityModel.Should().Be("Course");
                            b.IsNullable.Should().BeFalse();
                        });
                });
        }
    }
}
