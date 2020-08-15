using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dash.Engine.Abstractions;
using Dash.Exceptions;
using Dash.Nodes;

namespace Dash.Engine
{
    public class DefaultSemanticAnalyzer : ISemanticAnalyzer
    {
        private readonly IDataTypeParser _dataTypeParser;
        private readonly List<string> _errors = new List<string>();

        public IEnumerable<string> Errors => _errors;

        public DefaultSemanticAnalyzer(IDataTypeParser dataTypeParser)
        {
            _dataTypeParser = dataTypeParser;
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
                _errors.Add("Entity name must be alphanumeric and cannot start with a number");
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
            // Validate that referenced entity exists
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
