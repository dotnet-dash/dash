using Dash.Engine.Parsers.Result;

namespace Dash.Engine
{
    public interface IDataTypeParser
    {
        DataTypeParserResult Parse(string dataTypeSpecification);
    }
}