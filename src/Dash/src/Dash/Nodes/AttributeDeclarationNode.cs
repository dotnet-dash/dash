namespace Dash.Nodes
{
    public class AttributeDeclarationNode : AstNode
    {
        public AttributeDeclarationNode(string attributeName, string attributeDataType)
        {
            AttributeName = attributeName;
            AttributeDataType = attributeDataType;
        }

        public string AttributeName { get; set; }

        public string AttributeDataType { get; set; }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}