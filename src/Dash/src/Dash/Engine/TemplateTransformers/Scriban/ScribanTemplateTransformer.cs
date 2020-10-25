// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Extensions;
using Microsoft.Extensions.Options;
using Scriban;
using Scriban.Runtime;

namespace Dash.Engine.TemplateTransformers.Scriban
{
    public class ScribanTemplateTransformer : ITemplateTransformer
    {
        private readonly DashOptions _options;

        public ScribanTemplateTransformer(IOptions<DashOptions> options)
        {
            _options = options.Value;
        }

        public async Task<string> Transform(TemplateOptions options)
        {
            var template = Template.Parse(options.TemplateText);

            var scriptObject = new ScriptObject
            {
                {"namespace", _options.DefaultNamespace!},
                {"modelName", Path.GetFileNameWithoutExtension(_options.InputFile!).StartWithCapitalLetter()},
                {"entities", options.Entities},
                {"pluralize", options.Pluralize},
            };
            scriptObject.Import(typeof(CSharpOutputHelpers));

            var context = new TemplateContext();
            context.PushGlobal(scriptObject);

            return await template.RenderAsync(context);
        }
    }
}
