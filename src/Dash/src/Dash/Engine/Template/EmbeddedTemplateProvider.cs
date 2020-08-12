﻿using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Exceptions;

namespace Dash.Engine.Template
{
    public class EmbeddedTemplateProvider : ITemplateProvider
    {
        public Task<string> GetTemplate(string templateName)
        {
            using var stream = Assembly
                .GetAssembly(typeof(EmbeddedTemplateProvider))!
                .GetManifestResourceStream("Dash.Templates." + templateName);

            if (stream == null)
            {
                throw new EmbeddedTemplateNotFoundException(templateName);
            }

            TextReader tr = new StreamReader(stream);
            return tr.ReadToEndAsync();
        }
    }
}