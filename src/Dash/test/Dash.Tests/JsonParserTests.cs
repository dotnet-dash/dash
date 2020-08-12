using System.IO;
using Dash.Engine.JsonParser;
using FluentAssertions;
using Xunit;

namespace Dash.Tests
{
    public class JsonParserTests
    {
        [Fact]
        public void Parse_EmptyJson()
        {
            // Arrange
            var parser = new JsonParser(null);

            // Act
            var result = parser.Parse("{}");
        }

        [Fact]
        public void Parse_EmptyModelJson_ShouldHaveParsedModelWithBaseEntityOnly()
        {
            // Arrange
            var parser = new JsonParser(null);

            // Act
            var result = parser.Parse(File.ReadAllText("Samples/EmptyModel.json"));

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
                            a.CodeDataType.Should().Be("Int");
                        });
                    first.SingleReferences.Should().BeEmpty();
                    first.CollectionReferences.Should().BeEmpty();
                });
        }

        [Fact]
        public void Parse_HelloWorldJson_ShouldHaveCreatedModelWithoutErrors()
        {
            // Arrange
            var parser = new JsonParser(null);

            // Act
            var result = parser.Parse(File.ReadAllText("Samples/HelloWorld.json"));

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
                            a.CodeDataType.Should().Be("Int");
                        },
                        b =>
                        {
                            b.Name.Should().Be("Email");
                            b.CodeDataType.Should().Be("Email");
                        });
                });
        }

        [Fact]
        public void Parse_OverrideBaseIdJson_BaseOverridden()
        {
            // Arrange
            var sut = new JsonParser(null);

            // Act
            var result = sut.Parse(File.ReadAllText("Samples/OverrideBaseId.json"));

            // Assert
            result.Entities.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Base");
                    first.Attributes.Should().SatisfyRespectively(a =>
                    {
                        a.Name.Should().Be("Id");
                        a.CodeDataType.Should().Be("Guid");
                    });
                });
        }

        [Fact]
        public void Parse_HasAndBelongsToManyJson_ShouldHaveParsedModelWithoutErrors()
        {
            // Arrange
            var parser = new JsonParser(null);

            // Act
            var result = parser.Parse(File.ReadAllText("Samples/HasAndBelongsToMany.json"));

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
                        a.CodeDataType.Should().Be("Int");
                    });
                },
                third =>
                {
                    third.Name.Should().Be("Product");
                    third.Attributes.Should().SatisfyRespectively(a =>
                    {
                        a.Name.Should().Be("Id");
                        a.CodeDataType.Should().Be("Int");
                    }, b =>
                    {
                        b.Name.Should().Be("Description");
                        b.CodeDataType.Should().Be("String");
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
            // Arrange
            var parser = new JsonParser(null);

            // Act
            var result = parser.Parse(File.ReadAllText("Samples/HasMany.json"));

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
