namespace Dash.Nodes
{
    public class Attribute
    {
        public Attribute(string name, string? dataType) : this(name, dataType, null)
        {
        }

        public Attribute(string name, string? dataType, string? dataTypeRegularExpression)
        {
            Name = name;
            DataType = dataType;
            DataTypeRegularExpression = dataTypeRegularExpression;
        }

        public string Name { get; }

        public string? DefaultValue { get; set; }

        public string? DataType { get; }

        public string? DataTypeRegularExpression { get; set; }

        public bool IsNullable { get; set; }

        public int? RangeLowerBound { get; set; }

        public int? RangeUpperBound { get; set; }

        public int? Length { get; set; }
    }
}