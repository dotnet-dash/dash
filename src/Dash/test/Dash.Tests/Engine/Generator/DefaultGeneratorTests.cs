using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Engine.Generator;
using Dash.Engine.Models.SourceCode;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine.Generator
{
    public class DefaultGeneratorTests
    {
        [Fact]
        public async Task Generate_HappyFlow_ShouldHaveWrittenFileToFileSystem()
        {
            // Arrange
            var templateProvider = Substitute.For<ITemplateProvider>();
            templateProvider.GetTemplate("poco").Returns("poco template");
            templateProvider.GetTemplate("poco2").Returns("poco template 2");

            var fileSystem = new MockFileSystem();

            var modelRepository = Substitute.For<IModelRepository>();

            var sut = new DefaultGenerator(
                templateProvider,
                fileSystem,
                modelRepository,
                Substitute.For<IConsole>());

            var configuration = new Configuration
            {
                Templates = new[]
                {
                    new TemplateNode
                    {
                        Output = @"c:\temp\",
                        Template = "poco"
                    },
                    new TemplateNode
                    {
                        Output = @"c:\temp\",
                        Template = "poco2"
                    }
                },
            };

            var sourceCodeDocument = new SourceCodeDocument(configuration, new ModelNode());

            // Act
            await sut.Generate(sourceCodeDocument);

            // Assert
            fileSystem.GetFile(@"c:\temp\poco.generated.cs").TextContents.Should().Be("poco template");
            fileSystem.GetFile(@"c:\temp\poco2.generated.cs").TextContents.Should().Be("poco template 2");
        }
    }
}
