using Dash.Engine.JsonParser;

namespace Dash.Engine.Abstractions
{
    public interface IDataTypeParser
    {
        DataTypeParserResult Parse(string dataTypeSpecification);
    }
}