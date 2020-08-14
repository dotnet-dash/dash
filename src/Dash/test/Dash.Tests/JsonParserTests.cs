using System.Collections.Generic;
using System.IO;
using Dash.Engine.Abstractions;
using Dash.Engine.JsonParser;
using Dash.Engine.LanguageProviders;
using FluentAssertions;
using Xunit;

namespace Dash.Tests
{
    public class JsonParserTests
    {
        private readonly JsonParser _sut;

        public JsonParserTests()
        {
            var dataTypeParser = new DataTypeParser(new List<ILanguageProvider>
            {
                new CSharpLanguageProvider(),
                new SqlServerLanguageProvider(),
            });

            _sut = new JsonParser(dataTypeParser);
        }

        [Fact]
        public void Parse_EmptyJson()
        {
            // Act
            var result = _sut.Parse("{}");

            // Assert
            result.Entities.Should().BeEmpty();
        }

        [Fact]
        public void Parse_EmptyModelJson_ShouldHaveParsedModelWithBaseEntityOnly()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/EmptyModel.json"));

            // Assert
            result.Entities.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Base");
                    first.Inherits.Should().BeNull();
                    first.Attributes.Should().SatisfyRespectively(
                        a =>
                        {
                            a.Name.Should().Be("Id");
                            a.CodeDataType.Should().Be("int");
                            a.DatabaseDataType.Should().Be("int");
                        });
                    first.SingleReferences.Should().BeEmpty();
                    first.CollectionReferences.Should().BeEmpty();
                });
        }

        [Fact]
        public void Parse_HelloWorldJson_ShouldHaveCreatedModelWithoutErrors()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/HelloWorld.json"));

            // Assert
            result.Entities.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Base");
                },
                second =>
                {
                    second.Name.Should().Be("Account");
                    second.Inherits.Should().Be("Base");
                    second.Attributes.Should().SatisfyRespectively(
                        a =>
                        {
                            a.Name.Should().Be("Id");
                            a.CodeDataType.Should().Be("int");
                            a.DatabaseDataType.Should().Be("int");
                        },
                        b =>
                        {
                            b.Name.Should().Be("Email");
                            b.CodeDataType.Should().Be("string");
                            b.DatabaseDataType.Should().Be("nvarchar");
                        });
                });
        }

        [Fact]
        public void Parse_OverrideBaseIdJson_BaseOverridden()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/OverrideBaseId.json"));

            // Assert
            result.Entities.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Base");
                    first.Attributes.Should().SatisfyRespectively(a =>
                    {
                        a.Name.Should().Be("Id");
                        a.CodeDataType.Should().Be("System.Guid");
                        a.DatabaseDataType.Should().Be("uniqueidentifier");
                    });
                });
        }

        [Fact]
        public void Parse_HasAndBelongsToManyJson_ShouldHaveParsedModelWithoutErrors()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/HasAndBelongsToMany.json"));

            // Assert
            result.Entities.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Base");
                },
                second =>
                {
                    second.Name.Should().Be("Order");
                    second.Attributes.Should().SatisfyRespectively(a =>
                    {
                        a.Name.Should().Be("Id");
                        a.CodeDataType.Should().Be("int");
                        a.DatabaseDataType.Should().Be("int");
                    });
                },
                third =>
                {
                    third.Name.Should().Be("Product");
                    third.Attributes.Should().SatisfyRespectively(a =>
                    {
                        a.Name.Should().Be("Id");
                        a.CodeDataType.Should().Be("int");
                        a.DatabaseDataType.Should().Be("int");
                    }, b =>
                    {
                        b.Name.Should().Be("Description");
                        b.CodeDataType.Should().Be("string");
                        b.DatabaseDataType.Should().Be("varchar");
                    });
                },
                fourth =>
                {
                    fourth.Name.Should().Be("OrderProduct");
                    fourth.Attributes.Should().SatisfyRespectively(a =>
                    {
                        a.Name.Should().Be("Id");
                    });
                    fourth.SingleReferences.Should().SatisfyRespectively(a =>
                    {
                        a.Name.Should().Be("Order");
                    }, b =>
                    {
                        b.Name.Should().Be("Product");
                    });
                });
        }

        [Fact]
        public void Parse_HasMany_ShouldHaveParsedModelWithoutError()
        {
            // Act
            var result = _sut.Parse(File.ReadAllText("Samples/HasMany.json"));

            // Assert
            result.Entities.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Base");
                },
                second =>
                {
                    second.Name.Should().Be("Order");
                    second.CollectionReferences.Should().ContainSingle(e => e.Name == "OrderLine");
                },
                third =>
                {
                    third.Name.Should().Be("OrderLine");
                    third.SingleReferences.Should().ContainSingle(e => e.Name == "Order");
                }
            );
        }
    }
}
