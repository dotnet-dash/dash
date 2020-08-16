using Dash.Nodes;

namespace Dash.Engine.Models.SourceCode
{
    public class Configuration
    {
        public string AutogenSuffix { get; set; } = ".generated";

        public string Header { get; set; } = string.Empty;

        public string DefaultNamespace { get; set; } = string.Empty;

        public TemplateNode[] Templates { get; set; } = new[]
        {
            new TemplateNode
            {
                Template = "efpoco"
            },

            new TemplateNode
            {
                Template = "efcontext"
            }
        };
    }
}
