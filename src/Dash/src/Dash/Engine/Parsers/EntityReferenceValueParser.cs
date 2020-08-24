// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine.Parsers.Result;

namespace Dash.Engine.Parsers
{
    public class EntityReferenceValueParser : IEntityReferenceValueParser
    {
        public EntityReferenceValueParserResult Parse(string entityReferenceValue)
        {
            var result = new EntityReferenceValueParserResult
            {
                IsNullable = entityReferenceValue.EndsWith("?")
            };

            if (result.IsNullable)
            {
                entityReferenceValue = entityReferenceValue[..^1];
            }

            result.EntityName = entityReferenceValue;
            return result;
        }
    }
}
