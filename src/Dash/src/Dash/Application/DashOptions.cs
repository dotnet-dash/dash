﻿// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace Dash.Application
{
    public class DashOptions
    {
        public bool Verbose { get; set; }

        public string WorkingDirectory { get; set; } = ".";

        public string? ProjectFile { get; set; }

        public string? InputFile { get; set; }
    }
}
