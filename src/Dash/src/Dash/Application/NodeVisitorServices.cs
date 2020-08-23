using Dash.Engine;
using Dash.Engine.Visitors;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public static class NodeVisitorServices
    {
        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<INodeVisitor, CreateJoinedEntityVisitor>();
            services.AddSingleton<INodeVisitor, SetInheritanceVisitor>();
            services.AddSingleton<INodeVisitor, DefaultSymbolCollector>();
            services.AddSingleton<INodeVisitor, DefaultSemanticAnalyzer>();
            services.AddSingleton<INodeVisitor, IoAnalyzer>();
            services.AddSingleton<INodeVisitor, DefaultModelBuilder>();
            services.AddSingleton<INodeVisitor, ReferenceModelBuilder>();
            services.AddSingleton<INodeVisitor, UriResourceDownload>();
            services.AddSingleton<INodeVisitor, ModelSeedBuilder>();
        }
    }
}
