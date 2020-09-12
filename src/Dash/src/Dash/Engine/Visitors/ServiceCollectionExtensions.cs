using Microsoft.Extensions.DependencyInjection;

namespace Dash.Engine.Visitors
{
    public static class ServiceCollectionExtensions
    {
        public static void AddNodeVisitors(this IServiceCollection services)
        {
            services.AddSingleton<INodeVisitor, CreateJoinedEntityVisitor>();
            services.AddSingleton<INodeVisitor, SetInheritanceVisitor>();
            services.AddSingleton<INodeVisitor, DefaultSymbolCollector>();
            services.AddSingleton<INodeVisitor, ValidateConfigurationVisitor>();
            services.AddSingleton<INodeVisitor, DefaultSemanticAnalyzer>();
            services.AddSingleton<INodeVisitor, ValidateUriVisitor>();
            services.AddSingleton<INodeVisitor, DefaultModelBuilder>();
            services.AddSingleton<INodeVisitor, ReferenceModelBuilder>();
            services.AddSingleton<INodeVisitor, UriResourceDownload>();
            services.AddSingleton<INodeVisitor, ModelSeedBuilder>();
        }
    }
}
