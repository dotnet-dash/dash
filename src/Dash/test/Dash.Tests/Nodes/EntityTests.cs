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
            superEntity.Attributes.Add(new Attribute("Id", "Guid"));
            superEntity.Attributes.Add(new Attribute("Created", "DateTime"));
            superEntity.Attributes.Add(new Attribute("Name", "String"));

            var sut = new Entity("Person");
            sut.Attributes.Add(new Attribute("Name", "Unicode"));
            sut.Attributes.Add(new Attribute("GivenName", "String"));
            sut.Attributes.Add(new Attribute("Id", "Int"));

            // Act
            sut.InheritAttributes(superEntity);

            // Assert
            sut.Attributes.Should().SatisfyRespectively
            (
                first =>
                {
                    first.Name.Should().Be("Id");
                    first.DataType.Should().Be("Int");
                },
                second =>
                {
                    second.Name.Should().Be("Created");
                    second.DataType.Should().Be("DateTime");
                },
                third =>
                {
                    third.Name.Should().Be("Name");
                    third.DataType.Should().Be("Unicode");
                },
                fourth =>
                {
                    fourth.Name.Should().Be("GivenName");
                    fourth.DataType.Should().Be("String");
                }
            );
        }
    }
}
