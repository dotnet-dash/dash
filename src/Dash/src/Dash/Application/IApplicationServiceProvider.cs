﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace Dash.Application
{
    public interface IApplicationServiceProvider
    {
        IServiceProvider Create(bool verbose, string workingDir);
    }
}