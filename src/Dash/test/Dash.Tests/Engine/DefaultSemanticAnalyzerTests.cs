using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dash.Engine;
using Dash.Engine.Abstractions;
using Dash.Engine.Visitors;
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
        private readonly IDataTypeParser _parser = Substitute.For<IDataTypeParser>();
        private readonly ISymbolRepository _symbolRepository = Substitute.For<ISymbolRepository>();
        private readonly IReservedSymbolProvider _reservedSymbolProvider = Substitute.For<IReservedSymbolProvider>();
        private readonly ErrorRepository _errorRepository = new ErrorRepository();
        private readonly DefaultSemanticAnalyzer _sut;

        public DefaultSemanticAnalyzerTests()
        {
            _sut = new DefaultSemanticAnalyzer(
                _parser,
                _symbolRepository,
                _reservedSymbolProvider,
                Substitute.For<IConsole>(),
                _errorRepository);
        }

        [Fact]
        public async Task Visit_ModelNode_ShouldHaveVisitedEntityDeclarationNodes()
        {
            // Arrange
            var modelNode = new ModelNode();
            modelNode.EntityDeclarations.Add(Substitute.For<EntityDeclarationNode>(modelNode, "Account"));
            modelNode.EntityDeclarations.Add(Substitute.For<EntityDeclarationNode>(modelNode, "Person"));

            // Act
            await _sut.Visit(modelNode);

            // Assert
            await modelNode.EntityDeclarations[0].Received(1).Accept(_sut);
            await modelNode.EntityDeclarations[1].Received(1).Accept(_sut);
        }

        [Fact]
        public async Task Visit_ModelNode_ContainsDuplicateEntityName_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var modelNode = new ModelNode();
            modelNode.AddEntityDeclarationNode("Account");
            modelNode.AddEntityDeclarationNode("Account");

            // Act
            await _sut.Visit(modelNode);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Model contains duplicate declarations for entity 'Account'");
                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task Visit_EntityDeclarationNode_WithNullOrWhitespaceName_ShouldHaveUpdatedErrorRepository(string name)
        {
            // Arrange
            var entityDeclarationNode = new EntityDeclarationNode(new ModelNode(), name);

            // Act
            await _sut.Visit(entityDeclarationNode);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
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
        public async Task Visit_EntityDeclarationNode_WithNonAllowedCharacters_ShouldHaveUpdatedErrorRepository(string name)
        {
            // Arrange
            var entityDeclarationNode = new EntityDeclarationNode(new ModelNode(), name);

            // Act
            await _sut.Visit(entityDeclarationNode);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be($"'{name}' is an invalid name. You can only use alphanumeric characters, and it cannot start with a number");
                });
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_ContainsDuplicateAttributeNames_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var entityDeclarationNode = new EntityDeclarationNode(new ModelNode(), "Account");
            entityDeclarationNode.AddAttributeDeclaration("Id", "Int");
            entityDeclarationNode.AddAttributeDeclaration("Id", "Guid");

            // Act
            await _sut.Visit(entityDeclarationNode);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Entity 'Account' contains duplicate declarations for attribute 'Id'");
                });
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_InheritedEntityNotFound_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            _symbolRepository.EntityExists("User").Returns(false);

            var node = new EntityDeclarationNode(new ModelNode(), "Account");
            node.AddInheritanceDeclaration("User");

            // Act
            await _sut.Visit(node);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Entity 'Account' wants to inherit unknown entity 'User'");
                });
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_MultipleInheritanceDeclaration_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            _symbolRepository.EntityExists(Arg.Any<string>()).Returns(true);

            var node = new EntityDeclarationNode(new ModelNode(), "FooBar");
            node.AddInheritanceDeclaration("Foo");
            node.AddInheritanceDeclaration("Bar");

            // Act
            await _sut.Visit(node);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
            first =>
            {
                first.Should().Be("Multiple inheritance declaration found for 'FooBar'");
            });
        }

        [Fact]
        public async Task Visit_AttributeDeclarationNode_ThrowsInvalidDataTypeException_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            _parser.Parse("Invalid").Throws(e => new InvalidDataTypeException("Invalid"));

            var entity = new EntityDeclarationNode(new ModelNode(), "Foo");
            var node = new AttributeDeclarationNode(entity, "Id", "Invalid");

            // Act
            await _sut.Visit(node);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be("The specified datatype 'Invalid' is invalid"));
        }

        [Fact]
        public async Task Visit_AttributeDeclarationNode_ThrowsInvalidDataTypeConstraintException_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            _parser.Parse("Invalid").Throws(e => new InvalidDataTypeConstraintException("Invalid data type"));

            var entity = new EntityDeclarationNode(new ModelNode(), "Foo");
            var node = new AttributeDeclarationNode(entity, "Id", "Invalid");

            // Act
            await _sut.Visit(node);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first =>
                {
                    first.Should().Be("Invalid data type");
                });
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_SelfInheritance_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            _symbolRepository.EntityExists("Foo").Returns(true);

            var node = new EntityDeclarationNode(new ModelNode(), "Foo");
            node.AddInheritanceDeclaration("Foo");

            await _sut.Visit(node);

            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be("Self-inheritance not allowed: 'Foo'"));
        }

        [Fact]
        public async Task Visit_EntityDeclarationNode_ReservedEntityNameUsed_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            _reservedSymbolProvider.IsReservedEntityName("Foo").Returns(true);

            // Act
            await _sut.Visit(new EntityDeclarationNode(new ModelNode(), "Foo"));

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be("'Foo' is a reserved name and cannot be used as an entity name."));
        }

        [Fact]
        public async Task Visit_CsvSeedDeclarationNode_HeaderNameIsNotAnAttribute_ShouldHaveUpdatedErrorRepository()
        {
            // Arrange
            var node = new CsvSeedDeclarationNode(
                new EntityDeclarationNode(new ModelNode(), "Foo"),
                new Uri("https://unittest"),
                false,
                null,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "CsvHeader", "NonExistingAttribute" }
                });

            // Act
            await _sut.Visit(node);

            // Assert
            _errorRepository.GetErrors().Should().SatisfyRespectively(
                first => first.Should().Be("Trying to map header 'CsvHeader' to unknown Entity Attribute 'NonExistingAttribute'"));
        }
    }
}