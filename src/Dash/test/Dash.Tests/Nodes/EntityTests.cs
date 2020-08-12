using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Nodes
{
    public class EntityTests
    {
        [Fact]
        public void InheritAttributes()
        {
            // Arrange
            var superEntity = new Entity("Base");
            superEntity.Attributes.Add(new Attribute { Name = "Id", CodeDataType = "Guid" });
            superEntity.Attributes.Add(new Attribute { Name = "Created", CodeDataType = "DateTime" });
            superEntity.Attributes.Add(new Attribute { Name = "Name", CodeDataType = "String"});

            var sut = new Entity("Person");
            sut.Attributes.Add(new Attribute { Name = "Name", CodeDataType = "Unicode"});
            sut.Attributes.Add(new Attribute { Name = "GivenName", CodeDataType = "String"});
            sut.Attributes.Add(new Attribute { Name = "Id", CodeDataType = "Int"});

            // Act
            sut.InheritAttributes(superEntity);

            // Assert
            sut.Attributes.Should().SatisfyRespectively
            (
                first =>
                {
                    first.Name.Should().Be("Id");
                    first.CodeDataType.Should().Be("Int");
                },
                second =>
                {
                    second.Name.Should().Be("Created");
                    second.CodeDataType.Should().Be("DateTime");
                },
                third =>
                {
                    third.Name.Should().Be("Name");
                    third.CodeDataType.Should().Be("Unicode");
                },
                fourth =>
                {
                    fourth.Name.Should().Be("GivenName");
                    fourth.CodeDataType.Should().Be("String");
                }
            );
        }
    }
}
