using System;
using System.Text.RegularExpressions;
using Dash.Engine.Exceptions;
using Attribute = Dash.Nodes.Attribute;

namespace Dash.Engine.JsonParser
{
    public class DataTypeParser
    {
        public Attribute Parse(string name, string dataTypeSpecification)
        {
            if (TryFindMatch("^([a-zA-Z_0-9]+)", dataTypeSpecification, out var dataType, out var remainingSpecification))
            {
                var result = new Attribute(name, dataType);
                ParseConstraints(result, remainingSpecification);

                return result;
            }

            throw new ParserException($"The specified DataType is invalid: '{dataTypeSpecification}'");
        }

        private void ParseConstraints(Attribute attribute, string constraints)
        {
            var alreadyProcessed = "";

            while (constraints.Length > 0)
            {
                if (alreadyProcessed.Contains(constraints[0]))
                {
                    throw new Exception("Constraints cannot be defined more than once");
                }

                alreadyProcessed += constraints[0];

                switch (constraints[0])
                {
                    case ':':
                        attribute.DataTypeRegularExpression = constraints.Substring(1);
                        return;

                    case '?':
                        attribute.IsNullable = true;
                        constraints = constraints.Substring(1);
                        break;

                    case '[':
                        ParseLength(attribute, constraints, out constraints);
                        break;

                    case '(':
                        ParseDefaultValue(attribute, constraints, out constraints);
                        break;

                    default:
                        throw new Exception($"Unable to parse {constraints}");
                }
            }
        }

        private void ParseLength(Attribute attribute, string value, out string remaining)
        {
            if (TryFindMatch(@"^(\[\d+\])", value, out var foundValue, out remaining))
            {
                foundValue = foundValue![1..^1];
                attribute.Length = int.Parse(foundValue);
                return;
            }

            if (TryFindMatch(@"^(\[((\d+..\d*)|(\d*..\d+)|(\d+..\d+))\])", value, out foundValue, out remaining))
            {
                foundValue = foundValue![1..^1];
                var ranges = foundValue.Split("..");
                if (ranges.Length == 2)
                {
                    attribute.RangeLowerBound = ranges[0] == string.Empty ? (int?)null : int.Parse(ranges[0]);
                    attribute.RangeUpperBound = ranges[1] == string.Empty ? (int?)null : int.Parse(ranges[1]);
                }
            }
        }

        private void ParseDefaultValue(Attribute attribute, string value, out string remaining)
        {
            if (TryFindMatch(@"^(\(==.+\))", value, out var parsedValue, out remaining))
            {
                attribute.DefaultValue = parsedValue![3..^1];
            }
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
