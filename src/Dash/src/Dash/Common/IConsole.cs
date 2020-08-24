// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Dash.Common
{
    public interface IConsole
    {
        void Trace(string message);
        void Info(string message);
        void Error(string errorMessage);
    }
}