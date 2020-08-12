namespace Dash.Nodes
{
    public class Attribute
    {
        public string? Name { get; set; }

        public string? DefaultValue { get; set; }

        public string? CodeDataType { get; set; }

        public string? DatabaseDataType { get; set; }

        public string? DataTypeRegularExpression { get; set; }

        public bool IsNullable { get; set; }

        public int? RangeLowerBound { get; set; }

        public int? RangeUpperBound { get; set; }

        public int? Length { get; set; }
    }
}