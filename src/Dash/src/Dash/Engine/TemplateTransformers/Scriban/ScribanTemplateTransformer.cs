// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;
using System.Linq;
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
        private readonly IModelRepository _modelRepository;
        private readonly DashOptions _options;

        public ScribanTemplateTransformer(IModelRepository modelRepository, IOptions<DashOptions> options)
        {
            _modelRepository = modelRepository;
            _options = options.Value;
        }

        public async Task<string> Transform(string templateText)
        {
            var template = Template.Parse(templateText);

            var scriptObject = new ScriptObject
            {
                {"namespace", _options.DefaultNamespace!},
                {"modelName", Path.GetFileNameWithoutExtension(_options.InputFile!).StartWithCapitalLetter()},
                {"entities", _modelRepository.EntityModels.Where(e => e.CodeGeneration)}
            };
            scriptObject.Import(typeof(GetCSharpLiteralFormatter));

            var context = new TemplateContext();
            context.PushGlobal(scriptObject);

            return await template.RenderAsync(context);
        }
    }
}
