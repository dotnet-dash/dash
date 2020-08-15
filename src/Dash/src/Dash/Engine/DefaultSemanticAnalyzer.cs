using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dash.Engine.Abstractions;
using Dash.Exceptions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine
{
    public class DefaultSemanticAnalyzer : ISemanticAnalyzer
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

        public void Visit(ModelNode node)
        {
            ValidateDuplicateEntityDeclarations(node);

            foreach (var declaration in node.EntityDeclarations)
            {
                declaration.Accept(this);
            }
        }

        public void Visit(EntityDeclarationNode node)
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

            if (node.InheritedEntity != null &&
                !_symbolCollector.EntityExists(node.InheritedEntity))
            {
                _errors.Add($"Entity '{node.Name}' wants to inherit unknown entity '{node.InheritedEntity}'");
            }

            if (node.InheritedEntity != null && node.InheritedEntity.IsSame(node.Name))
            {
                _errors.Add($"Self-inheritance not allowed: '{node.Name}'");
            }

            ValidateDuplicateAttributeDeclarations(node);
        }

        public void Visit(AttributeDeclarationNode node)
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

        public void Visit(ReferenceDeclarationNode node)
        {
            if (!_symbolCollector.EntityExists(node.ReferencedEntity))
            {
                _errors.Add($"Referenced entity '{node.ReferencedEntity}' does not exist");
            }
        }

        public void Visit(HasReferenceDeclarationNode node)
        {
        }

        public void Visit(HasManyReferenceDeclarationNode node)
        {
        }

        public void Visit(HasAndBelongsToManyDeclarationNode node)
        {
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
