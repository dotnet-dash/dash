﻿using System;
using System.IO.Abstractions;
using Dash.Engine;
using Dash.Engine.Abstractions;
using Dash.Engine.Generator;
using Dash.Engine.JsonParser;
using Dash.Engine.LanguageProviders;
using Dash.Engine.TemplateProviders;
using Dash.Engine.Visitors;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public class ApplicationServiceProvider : IApplicationServiceProvider
    {
        public IServiceProvider Create(bool verbose)
        {
            var services = new ServiceCollection();
            services.Configure<DashOptions>(options =>
            {
                options.Verbose = verbose;
            });

            services.AddSingleton<DashApplication>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ISourceCodeParser, DefaultSourceCodeParser>();
            services.AddSingleton<IGenerator, DefaultGenerator>();
            services.AddSingleton<IErrorRepository, ErrorRepository>();
            services.AddSingleton<IModelRepository, DefaultModelRepository>();
            services.AddSingleton<IReservedSymbolProvider, DefaultReservedSymbolProvider>();
            services.AddSingleton<IConsole, DefaultConsole>();
            RegisterTemplateProviders(services);
            RegisterValueParsers(services);
            RegisterNodeVisitors(services);
            RegisterLanguageProviders(services);

            return services.BuildServiceProvider(true);
        }

        private static void RegisterTemplateProviders(ServiceCollection services)
        {
            services.AddSingleton<ITemplateProvider, EmbeddedTemplateProvider>();
        }

        private static void RegisterValueParsers(ServiceCollection services)
        {
            services.AddSingleton<IDataTypeParser, DataTypeParser>();
            services.AddSingleton<IEntityReferenceValueParser, EntityReferenceValueParser>();
        }

        private static void RegisterNodeVisitors(ServiceCollection services)
        {
            services.AddSingleton<INodeVisitor, CreateJoinedEntityVisitor>();
            services.AddSingleton<INodeVisitor, SetInheritanceVisitor>();
            services.AddSingleton<INodeVisitor, DefaultSymbolCollector>();
            services.AddSingleton<INodeVisitor, DefaultSemanticAnalyzer>();
            services.AddSingleton<INodeVisitor, DefaultModelBuilder>();
            services.AddSingleton<INodeVisitor, ReferenceModelBuilder>();

            services.AddSingleton<ISymbolCollector, DefaultSymbolCollector>();
        }

        private static void RegisterLanguageProviders(ServiceCollection services)
        {
            services.AddSingleton<ILanguageProvider, CSharpLanguageProvider>();
            services.AddSingleton<ILanguageProvider, SqlServerLanguageProvider>();
        }
    }
}