using System.IO;
using Dash.Engine;
using Dash.Engine.Abstractions;
using Dash.Exceptions;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Dash.Tests.Engine
{
    public class DefaultSemanticAnalyzerTests
    {
        [Fact]
        public void Visit_ModelNode_ShouldHaveVisitedEntityDeclarationNodes()
        {
            // Arrange
            var sut = new DefaultSemanticAnalyzer(Substitute.For<IDataTypeParser>());

            var modelNode = new ModelNode();
            modelNode.EntityDeclarations.Add(Substitute.For<EntityDeclarationNode>("Account"));
            modelNode.EntityDeclarations.Add(Substitute.For<EntityDeclarationNode>("Person"));

            // Act
            sut.Visit(modelNode);

            // Assert
            modelNode.EntityDeclarations[0].Received(1).Accept(sut);
            modelNode.EntityDeclarations[1].Received(1).Accept(sut);
        }

        [Fact]
        public void Visit_ModelNode_ContainsDuplicateEntityName_ShouldResultInError()
        {
            // Arrange
            var sut = new DefaultSemanticAnalyzer(Substitute.For<IDataTypeParser>());

                var modelNode = new ModelNode();
            modelNode.EntityDeclarations.Add(new EntityDeclarationNode("Account"));
            modelNode.EntityDeclarations.Add(new EntityDeclarationNode("Account"));

            // Act
            sut.Visit(modelNode);

            // Assert
            sut.Errors.Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Model contains duplicate declarations for entity 'Account'");
                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Visit_EntityDeclarationNode_WithNullOrWhitespaceName_ShouldResultInError(string name)
        {
            // Arrange
            var sut = new DefaultSemanticAnalyzer(Substitute.For<IDataTypeParser>());

            var entityDeclarationNode = new EntityDeclarationNode(name);

            // Act
            sut.Visit(entityDeclarationNode);

            // Assert
            sut.Errors.Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Entity name cannot be null, empty or contain only white-spaces");
                });
        }

        [Theory]
        [InlineData("1")]
        [InlineData("/")]
        [InlineData("%%")]
        [InlineData("1ab")]
        [InlineData("ab1$")]
        public void Visit_EntityDeclarationNode_WithNonAllowedCharacters_ShouldResultInError(string name)
        {
            // Arrange
            var sut = new DefaultSemanticAnalyzer(Substitute.For<IDataTypeParser>());

            var entityDeclarationNode = new EntityDeclarationNode(name);

            // Act
            sut.Visit(entityDeclarationNode);

            // Assert
            sut.Errors.Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Entity name must be alphanumeric and cannot start with a number");
                });
        }

        [Fact]
        public void Visit_EntityDeclarationNode_ContainsDuplicateAttributeNames_ShouldResultInError()
        {
            // Arrange
            var sut = new DefaultSemanticAnalyzer(Substitute.For<IDataTypeParser>());

            var entityDeclarationNode = new EntityDeclarationNode("Account");
            entityDeclarationNode.AddAttributeDeclaration("Id", "Int");
            entityDeclarationNode.AddAttributeDeclaration("Id", "Guid");

            // Act
            sut.Visit(entityDeclarationNode);

            // Assert
            sut.Errors.Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Entity 'Account' contains duplicate declarations for attribute 'Id'");
                });
        }

        [Fact]
        public void Visit_AttributeDeclarationNode_ThrowsInvalidDataTypeException_ShouldResultInError()
        {
            // Arrange
            var parser = Substitute.For<IDataTypeParser>();
            parser.Parse("Invalid").Returns(x => throw new InvalidDataTypeException("Invalid"));

            var sut = new DefaultSemanticAnalyzer(parser);

            // Act
            sut.Visit(new AttributeDeclarationNode(new EntityDeclarationNode("Parent"),  "Id", "Invalid"));

            // Assert
            sut.Errors.Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("The specified datatype 'Invalid' is invalid");
                });
        }

        //[Fact]
        //public void Analyze_InheritUnknownEntity_ShouldProduceError()
        //{
        //    // Arrange
        //    var sut = new DefaultSemanticAnalyzer();

        //    var model = new Model();
        //    model.Entities.Add(
        //        new Entity("MyEntity")
        //        {
        //            Inherits = "SuperEntity"
        //        }
        //    );

        //    // Act
        //    var result = sut.Analyze(model).ToList();

        //    // Assert
        //    result.Count().Should().Be(1);
        //    result.Should().Contain("Unknown Entity Inheritance: 'MyEntity' wants to inherit unknown entity 'SuperEntity'");
        //}

        //[Fact]
        //public void Analyze_EntityInheritingItself_ShouldProduceError()
        //{
        //    // Arrange
        //    var sut = new DefaultSemanticAnalyzer();

        //    var model = new Model();
        //    model.Entities.Add(
        //        new Entity("MyEntity")
        //        {
        //            Inherits = "MyEntity"
        //        }
        //    );

        //    // Act
        //    var result = sut.Analyze(model).ToList();

        //    // Assert
        //    result.Count().Should().Be(1);
        //    result.Should().Contain("Self-Inheritance Not Allowed: 'MyEntity'");
        //}
    }
}