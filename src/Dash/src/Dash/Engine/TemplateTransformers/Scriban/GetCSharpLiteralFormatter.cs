// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Engine.TemplateTransformers.Scriban
{
    public static class GetCSharpLiteralFormatter
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

            if (value is decimal decimalValue)
            {
                return $"{decimalValue}m";
            }

            var v = value.ToString()!.Replace("\"", "\\\"");
            return $"\"{v}\"";
        }
    }
}
