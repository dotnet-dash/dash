// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dash.Common;
using Dash.Exceptions;
using Dash.Extensions;
using Dash.Nodes;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
namespace Dash.Engine.Visitors
{
    public class DefaultSemanticAnalyzer : BaseVisitor
    {
        private readonly IDataTypeParser _dataTypeParser;
        private readonly ISymbolRepository _symbolRepository;
        private readonly IReservedSymbolProvider _reservedSymbolProvider;
        private readonly IErrorRepository _errorRepository;

        public DefaultSemanticAnalyzer(
            IDataTypeParser dataTypeParser,
            ISymbolRepository symbolRepository,
            IReservedSymbolProvider reservedSymbolProvider,
            IConsole console,
            IErrorRepository errorRepository) : base(console)
        {
            _dataTypeParser = dataTypeParser;
            _symbolRepository = symbolRepository;
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

            if (node.AbstractDeclarationNodes.Count() > 1)
            {
                _errorRepository.Add($"Multiple abstract declarations found for '{node.Name}'");
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
            if (!_symbolRepository.EntityExists(node.InheritedEntity))
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
            foreach (var (key, value) in node.MapHeaders)
            {
                if (!node.Parent.AttributeDeclarations.Any(e => e.AttributeName.IsSame(key)))
                {
                    _errorRepository.Add($"Trying to map header '{value}' to unknown Entity Attribute '{key}'");
                }
            }

            return base.Visit(node);
        }

        public override Task Visit(UriNode node)
        {
            var scheme = node.Uri.OriginalString.StartsWith(".")
                ? Uri.UriSchemeFile
                : node.Uri.Scheme;

            if (!node.SupportedSchemes.Contains(scheme, StringComparer.OrdinalIgnoreCase))
            {
                var supported = string.Join(", ", node.SupportedSchemes);
                _errorRepository.Add($"Unsupported scheme '{node.Uri.Scheme}' found in Uri {node.Uri}. Supported schemes: {supported}");
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
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
