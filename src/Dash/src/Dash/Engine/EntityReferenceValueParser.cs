using Dash.Engine.Abstractions;

namespace Dash.Engine
{
    public class EntityReferenceValueParser : IEntityReferenceValueParser
    {
        public EntityReferenceValueParserResult Parse(string entityReferenceValue)
        {
            var result = new EntityReferenceValueParserResult();

            result.IsNullable = entityReferenceValue.EndsWith("?");
            if (result.IsNullable)
            {
                entityReferenceValue = entityReferenceValue[..^1];
            }

            result.EntityName = entityReferenceValue;
            return result;
        }
    }
}
