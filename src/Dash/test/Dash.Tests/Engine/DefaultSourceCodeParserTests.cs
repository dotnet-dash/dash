using System.Collections.Generic;
using System.IO;
using Dash.Engine;
using Dash.Engine.Parsers;
using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine
{
    public class DefaultSourceCodeParserTests
    {
        private readonly DefaultSourceCodeParser _sut;

        public DefaultSourceCodeParserTests()
        {
            _sut = new DefaultSourceCodeParser();
        }

        [Fact]
        public void Parse_EmptyJson_ShouldHaveParsedTree()
        {
            // Act
            var result = _sut.Parse("{}");

            // Assert
            result.ModelNode.EntityDeclarations.Count.Should().Be(0);
        }

        [Fact]
        public void Parse_Configuration()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/Configuration.json"));

            // Assert
            result.ConfigurationNode.AutogenSuffix.Should().Be(".autogenerated");
            result.ConfigurationNode.Header.Should().Be("(c) 2020 Dash");
            result.ConfigurationNode.DefaultNamespace.Should().Be("MyApplication");
            result.ConfigurationNode.Templates.Should().SatisfyRespectively(
                first =>
                {
                    first.Template.Should().Be("file:///relative/MyTemplate/EfContext");
                    first.Output.Should().Be("file:///relative/ef");
                },
                second =>
                {
                    second.Template.Should().Be("file:///relative/MyTemplates/Poco");
                    second.Output.Should().Be("file:///relative");
                }
            );
        }

        [Fact]
        public void Parse_HelloWorldJson_ShouldHaveParsedTree()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/HelloWorld.json"));

            // Assert
            result.ModelNode.EntityDeclarations.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Account");
                    first.AttributeDeclarations.Should().SatisfyRespectively(
                        a =>
                        {
                            a.AttributeName.Should().Be("Id");
                            a.AttributeDataType.Should().Be("Int");
                        },
                        b =>
                        {
                            b.AttributeName.Should().Be("Email");
                            b.AttributeDataType.Should().Be("Email");
                        });
                });
        }

        [Fact]
        public void Parse_OverrideBaseIdJson_ShouldHaveParsedTree()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/OverrideBaseId.json"));

            // Assert
            result.ModelNode.EntityDeclarations.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Base");
                    first.AttributeDeclarations.Should().SatisfyRespectively(a =>
                    {
                        a.AttributeName.Should().Be("Id");
                        a.AttributeDataType.Should().Be("Guid");
                    });
                });
        }

        [Fact]
        public void Parse_Has_ShouldHaveParsedTree()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/Has.json"));

            // Assert
            result.ModelNode.EntityDeclarations.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Person");
                    first.Has.Should().SatisfyRespectively(
                        a =>
                        {
                            a.Name.Should().Be("CountryOfBirth");
                            a.ReferencedEntity.Should().Be("Country");
                        },
                        b =>
                        {
                            b.Name.Should().Be("CountryOfResidence");
                            b.ReferencedEntity.Should().Be("Country");
                        });
                },
                second =>
                {
                    second.Name.Should().Be("Country");
                    second.Has.Should().BeEmpty();
                    second.HasMany.Should().BeEmpty();
                    second.HasAndBelongsToMany.Should().BeEmpty();
                }
            );
        }

        [Fact]
        public void Parse_HasNullable_ShouldHaveParsedTree()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/HasNullable.json"));

            // Assert
            result.ModelNode.EntityDeclarations.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Person");
                    first.Has.Should().SatisfyRespectively(
                        a =>
                        {
                            a.Name.Should().Be("MotherTongue");
                            a.ReferencedEntity.Should().Be("Language");
                        },
                        b =>
                        {
                            b.Name.Should().Be("PreferredLanguage");
                            b.ReferencedEntity.Should().Be("Language?");
                        });
                },
                second =>
                {
                    second.Name.Should().Be("Language");
                    second.Has.Should().BeEmpty();
                    second.HasMany.Should().BeEmpty();
                    second.HasAndBelongsToMany.Should().BeEmpty();
                }
            );
        }

        [Fact]
        public void Parse_HasMany_ShouldHaveParsedTree()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/HasMany.json"));

            // Assert
            result.ModelNode.EntityDeclarations.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Order");
                    first.Has.Should().BeEmpty();
                    first.HasMany.Should().SatisfyRespectively(
                        a =>
                        {
                            a.Name.Should().Be("OrderLine");
                            a.ReferencedEntity.Should().Be("OrderLine");
                        });
                    first.HasAndBelongsToMany.Should().BeEmpty();
                },
                second =>
                {
                    second.Name.Should().Be("OrderLine");
                    second.AttributeDeclarations.Should().SatisfyRespectively(
                        a =>
                        {
                            a.AttributeName.Should().Be("Description");
                            a.AttributeDataType.Should().Be("String");
                        });
                }
            );
        }

        [Fact]
        public void Parse_HasAndBelongsToManyJson_ShouldHaveParsedTree()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/HasAndBelongsToMany.json"));

            // Assert
            result.ModelNode.EntityDeclarations.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Order");
                    first.AttributeDeclarations.Should().BeEmpty();
                    first.Has.Should().BeEmpty();
                    first.HasAndBelongsToMany.Should().SatisfyRespectively(
                        a =>
                        {
                            a.Name.Should().Be("Product");
                            a.ReferencedEntity.Should().Be("Product");
                        });
                },
                second =>
                {
                    second.Name.Should().Be("Product");
                    second.AttributeDeclarations.Should().SatisfyRespectively(
                        a =>
                        {
                            a.AttributeName.Should().Be("Description");
                            a.AttributeDataType.Should().Be("String");
                        });
                    second.Has.Should().BeEmpty();
                    second.HasMany.Should().BeEmpty();
                    second.HasAndBelongsToMany.Should().BeEmpty();
                });
        }

        [Fact]
        public void Parse_Seed_ShouldHaveParsedTree()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/Seed.json"));

            // Assert
            result.ModelNode.EntityDeclarations.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Currency");
                    first.ChildNodes.Should().SatisfyRespectively(
                        a =>
                        {
                            var node = a.Should().BeOfType<CsvSeedDeclarationNode>().Subject;
                            node.UriNode.Uri.Should().Be("https://currencycode");
                            node.FirstLineIsHeader.Should().BeTrue();
                            node.MapHeaders.Should().Contain(
                                new KeyValuePair<string, string>("code", "CurrencyCode"),
                                new KeyValuePair<string, string>("name", "CurrencyName"));
                        }
                    );
                }
            );
        }
    }
}