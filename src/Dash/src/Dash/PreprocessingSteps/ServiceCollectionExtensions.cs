// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Dash.PreprocessingSteps.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Dash.PreprocessingSteps
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPreprocessingSteps(this IServiceCollection services)
        {
            services.AddSingleton<IPreprocessingStep, DashOptionsValidator>();
            services.AddSingleton<IPreprocessingStep, FindProjectFile>();
            services.AddSingleton<IPreprocessingStep, ParseProjectFile>();
        }
    }
}
