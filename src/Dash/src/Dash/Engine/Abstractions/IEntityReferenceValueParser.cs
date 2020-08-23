using Dash.Engine.Parsers;

namespace Dash.Engine.Abstractions
{
    public interface IEntityReferenceValueParser
    {
        EntityReferenceValueParserResult Parse(string entityReferenceValue);
    }
}