// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Dash.Common
{
    public interface IHttpUriDownloader
    {
        Task<(bool Success, string? FileName, byte[]? Content)> Download(Uri uri);
    }
}