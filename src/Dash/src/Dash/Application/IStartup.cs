﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace Dash.Application
{
    public interface IStartup
    {
        IServiceCollection ConfigureServices(DashOptions dashOptions);
    }
}