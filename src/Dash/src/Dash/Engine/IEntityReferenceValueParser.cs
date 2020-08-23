using Dash.Engine.Parsers;
using Dash.Engine.Parsers.Result;

namespace Dash.Engine
{
    public interface IEntityReferenceValueParser
    {
        EntityReferenceValueParserResult Parse(string entityReferenceValue);
    }
}