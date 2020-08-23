﻿using Dash.Engine;
using Dash.Engine.Parsers;
using Dash.Engine.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public static class SourceCodeServices
    {
        public static void Add(IServiceCollection services)
        {
            services.AddSingleton<ISourceCodeParser, DefaultSourceCodeParser>();
            services.AddSingleton<IDataTypeParser, DataTypeParser>();
            services.AddSingleton<IEntityReferenceValueParser, EntityReferenceValueParser>();
            services.AddSingleton<IReservedSymbolProvider, DefaultReservedSymbolProvider>();
        }
    }
}