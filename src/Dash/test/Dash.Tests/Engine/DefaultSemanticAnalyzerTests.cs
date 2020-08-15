using Dash.Engine;
using Dash.Engine.Abstractions;
using Dash.Exceptions;
using Dash.Nodes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Dash.Tests.Engine
{
    public class DefaultSemanticAnalyzerTests
    {
        [Fact]
        public void Visit_ModelNode_ShouldHaveVisitedEntityDeclarationNodes()
        {
            // Arrange
            var sut = CreateDefaultSut();

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
            var sut = CreateDefaultSut();

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
            var sut = CreateDefaultSut();

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
            var sut = CreateDefaultSut();

            var entityDeclarationNode = new EntityDeclarationNode(name);

            // Act
            sut.Visit(entityDeclarationNode);

            // Assert
            sut.Errors.Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be($"'{name}' is an invalid name. You can only use alphanumeric characters, and it cannot start with a number");
                });
        }

        [Fact]
        public void Visit_EntityDeclarationNode_ContainsDuplicateAttributeNames_ShouldResultInError()
        {
            // Arrange
            var sut = CreateDefaultSut();

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
        public void Visit_EntityDeclarationNode_InheritedEntityNotFound_ShouldResultInError()
        {
            // Arrange
            var symbolCollector = Substitute.For<ISymbolCollector>();
            symbolCollector.EntityExists("User").Returns(false);

            var sut = new DefaultSemanticAnalyzer(
                Substitute.For<IDataTypeParser>(),
                symbolCollector,
                Substitute.For<IReservedSymbolProvider>());

            // Act
            sut.Visit(new EntityDeclarationNode("Account") { InheritedEntity = "User" });

            // Assert
            sut.Errors.Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Entity 'Account' wants to inherit unknown entity 'User'");
                });
        }

        [Fact]
        public void Visit_AttributeDeclarationNode_ThrowsInvalidDataTypeException_ShouldResultInError()
        {
            // Arrange
            var parser = Substitute.For<IDataTypeParser>();
            parser.Parse("Invalid").Returns(x => throw new InvalidDataTypeException("Invalid"));

            var sut = new DefaultSemanticAnalyzer(parser,
                Substitute.For<ISymbolCollector>(),
                Substitute.For<IReservedSymbolProvider>());

            // Act
            sut.Visit(new AttributeDeclarationNode(new EntityDeclarationNode("Parent"),  "Id", "Invalid"));

            // Assert
            sut.Errors.Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("The specified datatype 'Invalid' is invalid");
                });
        }

        [Fact]
        public void Visit_EntityDeclarationNode_SelfInheritance_ShouldResultInError()
        {
            // Arrange
            var symbolCollector = Substitute.For<ISymbolCollector>();
            symbolCollector.EntityExists("Account").Returns(true);

            var sut = new DefaultSemanticAnalyzer(
                Substitute.For<IDataTypeParser>(),
                symbolCollector,
                Substitute.For<IReservedSymbolProvider>());

            sut.Visit(new EntityDeclarationNode("Account") { InheritedEntity = "Account" });

            sut.Errors.Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Self-inheritance not allowed: 'Account'");
                });
        }

        [Fact]
        public void Visit_EntityDeclarationNode_ReservedEntityNameUsed_ShouldResultInError()
        {
            // Arrange
            var reservedSymbolProvider = Substitute.For<IReservedSymbolProvider>();
            reservedSymbolProvider.IsReservedEntityName("Account").Returns(true);

            var sut = new DefaultSemanticAnalyzer(
                Substitute.For<IDataTypeParser>(),
                Substitute.For<ISymbolCollector>(),
                reservedSymbolProvider);

            sut.Visit(new EntityDeclarationNode("Account"));

            sut.Errors.Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("'Account' is a reserved name and cannot be used as an entity name.");
                });
        }

        private static DefaultSemanticAnalyzer CreateDefaultSut()
        {
            return new DefaultSemanticAnalyzer(
                Substitute.For<IDataTypeParser>(),
                Substitute.For<ISymbolCollector>(),
                Substitute.For<IReservedSymbolProvider>());
        }
    }
}