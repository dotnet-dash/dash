using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Exceptions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class DefaultSemanticAnalyzer : BaseVisitor, ISemanticAnalyzer
    {
        private readonly IDataTypeParser _dataTypeParser;
        private readonly ISymbolCollector _symbolCollector;
        private readonly IReservedSymbolProvider _reservedSymbolProvider;
        private readonly IErrorRepository _errorRepository;

        public DefaultSemanticAnalyzer(
            IDataTypeParser dataTypeParser,
            ISymbolCollector symbolCollector,
            IReservedSymbolProvider reservedSymbolProvider,
            IConsole console,
            IErrorRepository errorRepository) : base(console)
        {
            _dataTypeParser = dataTypeParser;
            _symbolCollector = symbolCollector;
            _reservedSymbolProvider = reservedSymbolProvider;
            _errorRepository = errorRepository;
        }

        public override Task Visit(ModelNode node)
        {
            ValidateDuplicateEntityDeclarations(node);

            return base.Visit(node);
        }

        public override Task Visit(EntityDeclarationNode node)
        {
            if (string.IsNullOrWhiteSpace(node.Name))
            {
                _errorRepository.Add("Entity name cannot be null, empty or contain only white-spaces");
                return Task.CompletedTask;
            }

            if (!Regex.IsMatch(node.Name, "^([a-zA-Z]+[a-zA-Z0-9]*)$"))
            {
                _errorRepository.Add($"'{node.Name}' is an invalid name. You can only use alphanumeric characters, and it cannot start with a number");
            }

            if (_reservedSymbolProvider.IsReservedEntityName(node.Name))
            {
                _errorRepository.Add($"'{node.Name}' is a reserved name and cannot be used as an entity name.");
            }

            ValidateDuplicateAttributeDeclarations(node);

            if (node.InheritanceDeclarationNodes.Count() > 1)
            {
                _errorRepository.Add($"Multiple inheritance declaration found for '{node.Name}'");
            }

            return base.Visit(node);
        }

        public override Task Visit(AttributeDeclarationNode node)
        {
            try
            {
                _dataTypeParser.Parse(node.AttributeDataType);
            }
            catch (InvalidDataTypeException exception)
            {
                _errorRepository.Add(exception.Message);
            }
            catch (InvalidDataTypeConstraintException exception)
            {
                _errorRepository.Add(exception.Message);
            }

            return base.Visit(node);
        }

        public override Task Visit(InheritanceDeclarationNode node)
        {
            if (!_symbolCollector.EntityExists(node.InheritedEntity))
            {
                _errorRepository.Add($"Entity '{node.Parent.Name}' wants to inherit unknown entity '{node.InheritedEntity}'");
            }

            if (node.InheritedEntity.IsSame(node.Parent.Name))
            {
                _errorRepository.Add($"Self-inheritance not allowed: '{node.Parent.Name}'");
            }

            return base.Visit(node);
        }

        public override Task Visit(CsvSeedDeclarationNode node)
        {
            foreach (var pair in node.MapHeaders)
            {
                if (!node.Parent.AttributeDeclarations.Any(e => e.AttributeName.IsSame(pair.Value)))
                {
                    _errorRepository.Add($"Trying to map header '{pair.Key}' to unknown Entity Attribute '{pair.Value}'");
                }
            }

            return base.Visit(node);
        }

        private void ValidateDuplicateEntityDeclarations(ModelNode node)
        {
            var duplicateNames = node.EntityDeclarations
                .GroupBy(e => e.Name)
                .Where(e => e.Count() > 1)
                .Select(e => e.Key);

            foreach (var item in duplicateNames)
            {
                _errorRepository.Add($"Model contains duplicate declarations for entity '{item}'");
            }
        }

        private void ValidateDuplicateAttributeDeclarations(EntityDeclarationNode expression)
        {
            var duplicateAttributeNames = expression.AttributeDeclarations
                .GroupBy(e => e.AttributeName)
                .Where(e => e.Count() > 1)
                .Select(e => e.Key);

            foreach (var item in duplicateAttributeNames)
            {
                _errorRepository.Add($"Entity '{expression.Name}' contains duplicate declarations for attribute '{item}'");
            }
        }
    }
}
