using System.Collections.Generic;
using System.Linq;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine
{
    public class DefaultSemanticAnalyzer : ISemanticAnalyzer
    {
        private readonly HashSet<string> _analysisErrors = new HashSet<string>();

        public IEnumerable<string> Analyze(Model model)
        {
            _analysisErrors.Clear();

            foreach (var entity in model.Entities)
            {
                ValidateName(entity.Name);
                ValidateMultipleDeclaration(model, entity.Name);
            }

            ValidateInherits(model);

            return _analysisErrors;
        }

        private void ValidateName(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
            {
                _analysisErrors.Add("Entity Name Cannot Be Null, Empty, or Whitespaces");
            }
        }

        private void ValidateMultipleDeclaration(Model model, string entityName)
        {
            var count = model.Entities.Count(e => e.Name.IsSame(entityName));

            if (count > 1)
            {
                _analysisErrors.Add($"Entity Declared Multiple Times: '{entityName}' ({count} times)");
            }
        }

        private void ValidateInherits(Model model)
        {
            foreach (var entity in model.Entities.Where(e => e.Inherits != null))
            {
                if (entity.Name.IsSame(entity.Inherits))
                {
                    _analysisErrors.Add($"Self-Inheritance Not Allowed: '{entity.Name}'");
                }

                var nameOccurence = model.Entities.Count(e => e.Name.IsSame(entity.Inherits));
                if (nameOccurence == 0)
                {
                    _analysisErrors!.Add($"Unknown Entity Inheritance: '{entity.Name}' wants to inherit unknown entity '{entity.Inherits}'");
                }
            }
        }
    }
}
