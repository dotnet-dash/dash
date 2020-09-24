// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
            services.AddSingleton<INodeVisitor, ValidateUriExistsVisitor>();
            services.AddSingleton<INodeVisitor, DefaultModelBuilder>();
            services.AddSingleton<INodeVisitor, ReferenceModelBuilder>();
            services.AddSingleton<INodeVisitor, UriResourceDownload>();
            services.AddSingleton<INodeVisitor, ModelSeedBuilder>();
        }
    }
}
