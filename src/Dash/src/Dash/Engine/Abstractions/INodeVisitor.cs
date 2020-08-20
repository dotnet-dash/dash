using System.Threading.Tasks;
using Dash.Nodes;

namespace Dash.Engine.Abstractions
{
    public interface INodeVisitor
    {
        Task Visit(ModelNode node);
        Task Visit(EntityDeclarationNode node);
        Task Visit(AttributeDeclarationNode node);
        Task Visit(HasReferenceDeclarationNode node);
        Task Visit(HasManyReferenceDeclarationNode node);
        Task Visit(HasAndBelongsToManyDeclarationNode node);
        Task Visit(InheritanceDeclarationNode node);
        Task Visit(CsvSeedDeclarationNode node);
        Task Visit(UriNode node);
    }
}