// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Nodes;

namespace Dash.Application
{
    public interface ISourceCodeProcessor
    {
        Task WalkTree(SourceCodeNode sourceCodeNode);
    }
}