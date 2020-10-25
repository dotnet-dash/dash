// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Globalization;
using Dash.Engine.Models;

namespace Dash.Engine.TemplateTransformers.Scriban
{
    public static class CSharpOutputHelpers
    {
        private static readonly Inflector.Inflector Inflector = new Inflector.Inflector(new CultureInfo("en-US"));
        public static string GetCSharpLiteral(object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is int intValue)
            {
                return intValue.ToString();
            }

            if (value is bool boolValue)
            {
                return boolValue.ToString().ToLower();
            }

            if (value is decimal decimalValue)
            {
                return $"{decimalValue}m";
            }

            var v = value.ToString()!.Replace("\"", "\\\"");
            return $"\"{v}\"";
        }

        public static string GetPropertyDefaultValueAssignment(object value)
        {
            const string code = " = null!;";

            if (value is ReferencedEntityModel referencedEntity)
            {
                if (!referencedEntity.IsNullable)
                {
                    return code;
                }
            }
            else if (value is AttributeModel attribute)
            {
                if (attribute.DataType.IsNumeric ||
                    attribute.DataType.IsDateTime)
                {
                    return string.Empty;
                }

                if (attribute.DataType.IsBoolean)
                {
                    if (attribute.DefaultValue != null)
                    {
                        var booleanValue = GetCSharpLiteral(bool.Parse(attribute.DefaultValue));
                        return $"= {booleanValue};";
                    }

                    return string.Empty;
                }

                if (!attribute.IsNullable)
                {
                    return code;
                }
            }

            return string.Empty;
        }

        public static string? FormatName(object value, bool pluralize)
        {
            if (pluralize && value is string s)
            {
                return Inflector.Pluralize(s);
            }

            return value?.ToString();
        }
    }
}
