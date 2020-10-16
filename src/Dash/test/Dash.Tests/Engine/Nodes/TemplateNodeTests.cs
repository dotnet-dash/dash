using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine.Nodes
{
    public class TemplateNodeTests
    {
        [Fact]
        public void Ctor_ByDefault_TemplateShouldBeNull()
        {
            // Act
            var sut = new TemplateNode();

            // Assert
            sut.Template.Should().BeNull();
        }

        [Fact]
        public void Ctor_ByDefault_OutputShouldBeNull()
        {
            // Act
            var sut = new TemplateNode();

            // Assert
            sut.Output.Should().BeNull();
        }

        [Fact]
        public void Ctor_ByDefault_OneClassPerFileShouldBeFalse()
        {
            // Act
            var sut = new TemplateNode();

            // Assert
            sut.OneClassPerFile.Should().BeFalse();
        }
    }
}
