using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        private readonly List<string> _errors = new List<string>();

        public IEnumerable<string> Errors => _errors;

        public DefaultSemanticAnalyzer(
            IDataTypeParser dataTypeParser,
            ISymbolCollector symbolCollector,
            IReservedSymbolProvider reservedSymbolProvider)
        {
            _dataTypeParser = dataTypeParser;
            _symbolCollector = symbolCollector;
            _reservedSymbolProvider = reservedSymbolProvider;
        }

        public override void Visit(ModelNode node)
        {
            ValidateDuplicateEntityDeclarations(node);

            base.Visit(node);
        }

        public override void Visit(EntityDeclarationNode node)
        {
            if (string.IsNullOrWhiteSpace(node.Name))
            {
                _errors.Add("Entity name cannot be null, empty or contain only white-spaces");
                return;
            }

            if (!Regex.IsMatch(node.Name, "^([a-zA-Z]+[a-zA-Z0-9]*)$"))
            {
                _errors.Add($"'{node.Name}' is an invalid name. You can only use alphanumeric characters, and it cannot start with a number");
            }

            if (_reservedSymbolProvider.IsReservedEntityName(node.Name))
            {
                _errors.Add($"'{node.Name}' is a reserved name and cannot be used as an entity name.");
            }

            ValidateDuplicateAttributeDeclarations(node);

            if (node.InheritanceDeclarationNodes.Count() > 1)
            {
                _errors.Add($"Multiple inheritance declaration found for '{node.Name}'");
            }

            foreach (var item in node.InheritanceDeclarationNodes)
            {
                if (!_symbolCollector.EntityExists(item.InheritedEntity))
                {
                    _errors.Add($"Entity '{node.Name}' wants to inherit unknown entity '{item.InheritedEntity}'");
                }

                if (item.InheritedEntity.IsSame(node.Name))
                {
                    _errors.Add($"Self-inheritance not allowed: '{node.Name}'");
                }
            }
        }

        public override void Visit(AttributeDeclarationNode node)
        {
            try
            {
                _dataTypeParser.Parse(node.AttributeDataType);
            }
            catch (InvalidDataTypeException exception)
            {
                _errors.Add(exception.Message);
            }
        }

        public override void Visit(InheritanceDeclarationNode node)
        {
            if (!_symbolCollector.EntityExists(node.InheritedEntity))
            {
                _errors.Add($"Cannot inherit unknown entity '{node.InheritedEntity}'");
            }
        }

        private void ValidateDuplicateEntityDeclarations(ModelNode node)
        {
            var duplicateNames = node.EntityDeclarations
                .GroupBy(e => e.Name)
                .Where(e => e.Count() > 1)
                .Select(e => e.Key);

            foreach (var item in duplicateNames)
            {
                _errors.Add($"Model contains duplicate declarations for entity '{item}'");
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
                _errors.Add($"Entity '{expression.Name}' contains duplicate declarations for attribute '{item}'");
            }
        }
    }
}
