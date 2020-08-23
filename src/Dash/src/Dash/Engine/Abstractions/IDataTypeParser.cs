using Dash.Engine.Parsers;

namespace Dash.Engine.Abstractions
{
    public interface IDataTypeParser
    {
        DataTypeParserResult Parse(string dataTypeSpecification);
    }
}