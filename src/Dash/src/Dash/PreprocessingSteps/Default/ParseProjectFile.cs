// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Dash.Application;
using Dash.Roslyn;
using Microsoft.Extensions.Options;

namespace Dash.PreprocessingSteps.Default
{
    public class ParseProjectFile : IPreprocessingStep
    {
        private readonly IWorkspace _workspace;
        private readonly DashOptions _options;

        public ParseProjectFile(IOptions<DashOptions> options, IWorkspace workspace)
        {
            _workspace = workspace;
            _options = options.Value;
        }

        public async Task<bool> Process()
        {
            var project = await _workspace.OpenProjectAsync();
            if (project == null)
            {
                return false;
            }

            _options.DefaultNamespace = project.Namespace;

            return true;
        }
    }
}
