// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.Engine;
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
