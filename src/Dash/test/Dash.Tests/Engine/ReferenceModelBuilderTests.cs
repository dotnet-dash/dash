using Dash.Engine;
using Dash.Engine.Models;
using Dash.Engine.Visitors;
using Dash.Nodes;
using FluentAssertions;
using Xunit;

namespace Dash.Tests.Engine
{
    public class ReferenceModelBuilderTests
    {
        private readonly DefaultModelRepository _modelRepository;
        private readonly ReferenceModelBuilder _sut;

        public ReferenceModelBuilderTests()
        {
            _modelRepository = new DefaultModelRepository();
            _sut = new ReferenceModelBuilder(_modelRepository, new EntityReferenceValueParser());
        }

        //[Fact]
        //public void Visit_ModelNode_EntityModelCreated()
        //{
        //    // Arrange
        //    var modelRepository = new DefaultModelRepository();

        //    var sut = new DefaultModelBuilder(
        //        new DataTypeParser(),
        //        new List<ILanguageProvider>
        //        {
        //            new CSharpLanguageProvider(),
        //            new SqlServerLanguageProvider()
        //        },
        //        modelRepository);

        //    var countryNode = new EntityDeclarationNode("Country");
        //    countryNode.AddAttributeDeclaration("Id", "Int");

        //    var personNode = new EntityDeclarationNode("Person");
        //    personNode.AddHasDeclaration("CountryOfBirth", "Country");
        //    personNode.AddHasDeclaration("CountryOfResidence", "Country");

        //    var modelNode = new ModelNode
        //    {
        //        EntityDeclarations =
        //        {
        //            countryNode,
        //            personNode
        //        }
        //    };

        //    // Act
        //    sut.Visit(modelNode);

        //    // Assert
        //    modelRepository.EntityModels.Should().SatisfyRespectively(
        //        first =>
        //        {
        //            first.Name.Should().Be("Country");
        //            first.SingleReferences.Should().BeEmpty();
        //            first.CollectionReferences.Should().BeEmpty();
        //        },
        //        second =>
        //        {
        //            second.Name.Should().Be("Person");
        //            second.SingleReferences.Should().SatisfyRespectively(
        //                a =>
        //                {
        //                    a.ReferenceName.Should().Be("CountryOfBirth");
        //                },
        //                b =>
        //                {
        //                    b.ReferenceName.Should().Be("CountryOfResidence");
        //                });
        //        });
        //}

        [Theory]
        [InlineData("Language", false)]
        [InlineData("Language?", true)]
        public void Visit_HasReferenceDeclarationNode_ShouldHaveUpdatedEntities(string referencedEntity, bool expectedIsNullable)
        {
            // Arrange
            _modelRepository.Add(new EntityModel("Account"));
            var account = new EntityDeclarationNode("Account");

            _sut.Visit(account);

            var node = new HasReferenceDeclarationNode(
                account,
                "PreferredLanguage",
                referencedEntity);

            // Act
            _sut.Visit(node);

            // Assert
            _modelRepository.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.SingleReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("PreferredLanguage");
                            a.EntityModel.Should().Be("Language");
                            a.IsNullable.Should().Be(expectedIsNullable);
                        });
                });
        }

        [Fact]
        public void Visit_HasManyReferenceDeclarationNode_ShouldHaveUpdatedEntities()
        {
            // Arrange
            _modelRepository.CreateEntityModel("Order", "OrderLine");

            var account = new EntityDeclarationNode("Order");
            _sut.Visit(account);
            _sut.Visit(new EntityDeclarationNode("OrderLine"));

            var node = new HasManyReferenceDeclarationNode(account, "OrderLines", "OrderLine");

            // Act
            _sut.Visit(node);

            // Assert
            _modelRepository.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Order");
                    first.CollectionReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("OrderLines");
                            a.EntityModel.Should().Be("OrderLine");
                        });
                },
                second =>
                {
                    second.Name.Should().Be("OrderLine");
                    second.SingleReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("Order");
                            a.EntityModel.Should().Be("Order");
                        });
                });
        }

        [Fact]
        public void Visit_HasAndBelongsToManyDeclarationNode_ShouldHaveUpdatedEntities()
        {
            // Arrange
            _modelRepository.CreateEntityModel("Order", "Product");

            _sut.Visit(new EntityDeclarationNode("Order"));
            _sut.Visit(new EntityDeclarationNode("Product"));

            var node = new HasAndBelongsToManyDeclarationNode(new EntityDeclarationNode("Order"), "Product", "Product");

            // Act
            _sut.Visit(node);

            // Assert
            _modelRepository.EntityModels.Should().SatisfyRespectively(
                first =>
                {
                    first.Name.Should().Be("Order");
                    first.CollectionReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("OrderProduct");
                            a.EntityModel.Should().Be("OrderProduct");
                            a.IsNullable.Should().BeFalse();
                        });
                },
                second =>
                {
                    second.Name.Should().Be("Product");
                    second.CollectionReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("OrderProduct");
                            a.EntityModel.Should().Be("OrderProduct");
                            a.IsNullable.Should().BeFalse();
                        });
                },
                third =>
                {
                    third.Name.Should().Be("OrderProduct");
                    third.SingleReferences.Should().SatisfyRespectively(
                        a =>
                        {
                            a.ReferenceName.Should().Be("Order");
                            a.EntityModel.Should().Be("Order");
                            a.IsNullable.Should().BeFalse();
                        },
                        b =>
                        {
                            b.ReferenceName.Should().Be("Product");
                            b.EntityModel.Should().Be("Product");
                            b.IsNullable.Should().BeFalse();
                        });
                });
        }
    }
}
