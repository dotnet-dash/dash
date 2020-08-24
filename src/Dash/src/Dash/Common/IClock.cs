// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace Dash.Common
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}