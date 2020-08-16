namespace Dash.Engine.Models
{
    public class AttributeModel
    {
        public AttributeModel(string name, string dataType, bool isNullable, string? defaultValue)
        {
            Name = name;
            DataType = dataType;
            IsNullable = isNullable;
            DefaultValue = defaultValue;
        }

        public string Name { get; }

        public string DataType { get; }

        public string? DefaultValue { get; }

        public bool IsNullable { get; }
    }
}