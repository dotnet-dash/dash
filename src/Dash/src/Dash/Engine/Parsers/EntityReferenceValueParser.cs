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
