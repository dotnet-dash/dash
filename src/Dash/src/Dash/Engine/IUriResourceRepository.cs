// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Dash.Engine
{
    public interface IUriResourceRepository
    {
        Task Add(Uri uri);

        Task Add(Uri uri, string fileName, byte[] contents);

        Task<string> Get(Uri uri);

        Task<bool> Exists(Uri uri);

        Task<string> GetContents(Uri uri);

        Task<int> Count();
    }
}