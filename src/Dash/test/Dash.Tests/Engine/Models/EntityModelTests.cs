using Dash.Engine.Models;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Models
{
    public class EntityTests
    {
        [Fact]
        public void InheritAttributes()
        {
            // Arrange
            var superEntity = new EntityModel("Base");
            superEntity.CodeAttributes.Add(new AttributeModel("Id", "Guid", false, null));
            superEntity.CodeAttributes.Add(new AttributeModel("Created", "DateTime", false, null));
            superEntity.CodeAttributes.Add(new AttributeModel("Name", "String", false, null));

            var sut = new EntityModel("Person");
            sut.CodeAttributes.Add(new AttributeModel("Name", "Unicode", false, null));
            sut.CodeAttributes.Add(new AttributeModel("GivenName", "String", false, null));
            sut.CodeAttributes.Add(new AttributeModel("Id", "Int", false, null));

            // Act
            sut.InheritAttributes(superEntity);

            // Assert
            sut.CodeAttributes.Should().SatisfyRespectively
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
