// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine.Models;

namespace Dash.Engine.TemplateTransformers.Scriban
{
    public static class CSharpOutputHelpers
    {
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
            const string code = "= null!;";

            if (value is ReferencedEntityModel referencedEntity)
            {
                if (!referencedEntity.IsNullable)
                {
                    return code;
                }
            }
            else if (value is AttributeModel attribute)
            {
                if (attribute.DashDataType.IsNumeric ||
                    attribute.DashDataType.IsDateTime ||
                    attribute.DashDataType.IsBoolean)
                {
                    return string.Empty;
                }

                if (!attribute.IsNullable)
                {
                    return code;
                }
            }

            return string.Empty;
        }
    }
}
