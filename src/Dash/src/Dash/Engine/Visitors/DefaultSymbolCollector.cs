using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class DefaultSymbolCollector : BaseVisitor
    {
        private readonly ISymbolRepository _symbolRepository;

        public DefaultSymbolCollector(IConsole console, ISymbolRepository symbolRepository) : base(console)
        {
            _symbolRepository = symbolRepository;
        }

        public override Task Visit(EntityDeclarationNode node)
        {
            _symbolRepository.AddEntity(node.Name);
            Console.Trace($"Adding symbol: {node.Name}");

            return base.Visit(node);
        }

        public override Task Visit(AttributeDeclarationNode node)
        {
            _symbolRepository.AddEntityAttribute(node.Parent.Name, node.AttributeName);

            return base.Visit(node);
        }
    }
}
