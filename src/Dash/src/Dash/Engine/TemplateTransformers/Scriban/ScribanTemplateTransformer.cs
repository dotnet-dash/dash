// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Engine.Models;
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

        public async Task<string> Transform(string templateText, IEnumerable<EntityModel> entities)
        {
            var template = Template.Parse(templateText);

            var scriptObject = new ScriptObject
            {
                {"namespace", _options.DefaultNamespace!},
                {"modelName", Path.GetFileNameWithoutExtension(_options.InputFile!).StartWithCapitalLetter()},
                {"entities", entities}
            };
            scriptObject.Import(typeof(CSharpOutputHelpers));

            var context = new TemplateContext();
            context.PushGlobal(scriptObject);

            return await template.RenderAsync(context);
        }
    }
}
