// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Text;
using System.Text.RegularExpressions;
using Dash.Engine.DataTypes;
using Dash.Engine.Parsers.Result;
using Dash.Exceptions;

namespace Dash.Engine.Parsers
{
    public class DataTypeParser : IDataTypeParser
    {
        public DataTypeParserResult Parse(string dataTypeSpecification)
        {
            if (TryFindMatch("^([a-zA-Z0-9]+)", dataTypeSpecification, out var dashDataType, out var remainingSpecification))
            {
                var result = new DataTypeParserResult(DataTypeFactory.Create(dashDataType!));

                ParseConstraints(result, remainingSpecification);

                return result;
            }

            throw new InvalidDataTypeException(dataTypeSpecification);
        }

        private void ParseConstraints(DataTypeParserResult result, string constraints)
        {
            var alreadyProcessed = new StringBuilder();

            while (constraints.Length > 0)
            {
                if (alreadyProcessed.ToString().Contains(constraints[0]))
                {
                    throw new InvalidDataTypeConstraintException("Constraints cannot be defined more than once");
                }

                alreadyProcessed.Append(constraints[0]);

                switch (constraints[0])
                {
                    case ':':
                        result.DataTypeRegularExpression = constraints.Substring(1);
                        return;

                    case '?':
                        result.WithIsNullable(true);
                        constraints = constraints.Substring(1);
                        break;

                    case '[':
                        ParseLength(result, constraints, out constraints);
                        break;

                    case '(':
                        if (!ParseDefaultValue(result, constraints, out constraints))
                        {
                            throw new InvalidDataTypeException(constraints);
                        }
                        break;

                    default:
                        throw new InvalidDataTypeConstraintException($"Unable to parse {constraints}");
                }
            }
        }

        private void ParseLength(DataTypeParserResult result, string value, out string remaining)
        {
            if (TryFindMatch(@"^(\[\d+\])", value, out var foundValue, out remaining))
            {
                foundValue = foundValue![1..^1];
                result.Length = int.Parse(foundValue);
                return;
            }

            if (TryFindMatch(@"^(\[((\d+..\d*)|(\d*..\d+)|(\d+..\d+))\])", value, out foundValue, out remaining))
            {
                foundValue = foundValue![1..^1];
                var ranges = foundValue.Split("..");
                if (ranges.Length == 2)
                {
                    result.RangeLowerBound = ranges[0] == string.Empty ? (int?)null : int.Parse(ranges[0]);
                    result.RangeUpperBound = ranges[1] == string.Empty ? (int?)null : int.Parse(ranges[1]);
                }
            }
        }

        private bool ParseDefaultValue(DataTypeParserResult result, string value, out string remaining)
        {
            if (TryFindMatch(@"^(\(=='(.+)'\))", value, out var parsedValue, out remaining))
            {
                result.WithDefaultValue(parsedValue![4..^2]);
                return true;
            }

            return false;
        }

        private bool TryFindMatch(string pattern, string value, out string? foundValue, out string remaining)
        {
            var dataTypeMatch = Regex.Match(value, pattern);
            if (dataTypeMatch.Success)
            {
                remaining = Regex.Replace(value, pattern, "");
                foundValue = dataTypeMatch.Value;

                return true;
            }

            remaining = value;
            foundValue = null;
            return false;
        }
    }
}
