using System.Collections.Generic;
using Dash.Engine;
using Dash.Engine.Abstractions;
using Dash.Engine.JsonParser;
using Dash.Engine.LanguageProviders;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine
{
    public class DefaultModelBuilderTests
    {
        private readonly DefaultModelRepository _modelRepository;
        private readonly DefaultModelBuilder _sut;

        public DefaultModelBuilderTests()
        {
            _modelRepository = new DefaultModelRepository();
            _sut = new DefaultModelBuilder(
                new DataTypeParser(),
                new List<ILanguageProvider>
                {
                    new CSharpLanguageProvider(),
                    new SqlServerLanguageProvider()
                },
                _modelRepository);
        }

        [Fact]
        public void Visit_ModelNode_EntityModelCreated()
        {
            // Arrange
            var countryNode = new EntityDeclarationNode("Country");
            countryNode.AddAttributeDeclaration("Id", "Int");

            var personNode = new EntityDeclarationNode("Person");
            personNode.AddHasDeclaration("CountryOfBirth", "Country");
            personNode.AddHasDeclaration("CountryOfResidence", "Country");

            var modelNode = new ModelNode
            {
                EntityDeclarations =
                {
                    countryNode,
                    personNode
                }
            };

            // Act
            _sut.Visit(modelNode);

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
        public void Visit_EntityDeclarationNode_EntityModelCreated()
        {
            // Arrange
            var node = new EntityDeclarationNode("Account");
            node.AddAttributeDeclaration("Surname", "Unicode");
            node.AddAttributeDeclaration("Username", "String");
            node.AddAttributeDeclaration("Nickname", "Unicode?");

            // Act
            _sut.Visit(node);

            // Assert
            _modelRepository.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Account");
                    first.CodeAttributes.Should().SatisfyRespectively(
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

                    first.DataAttributes.Should().SatisfyRespectively(
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
    }
}
