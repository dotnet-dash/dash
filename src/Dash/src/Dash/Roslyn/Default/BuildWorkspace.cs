using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dash.Application;
using Dash.Common;
using Dash.Engine;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.Options;

namespace Dash.Roslyn.Default
{
    [ExcludeFromCodeCoverage]
    public class BuildWorkspace : IWorkspace
    {
        private readonly IConsole _console;
        private readonly DashOptions _options;
        private readonly MSBuildWorkspace _workspace;
        private readonly IErrorRepository _errorRepository;

        public BuildWorkspace(IOptions<DashOptions> options, IConsole console, IErrorRepository errorRepository)
        {
            _options = options.Value;
            _console = console;
            _errorRepository = errorRepository;

            if (!MSBuildLocator.IsRegistered)
            {
                MSBuildLocator.RegisterDefaults();
            }

            _workspace = MSBuildWorkspace.Create();
        }

        public async Task<IProject?> OpenProjectAsync()
        {
            _workspace.CloseSolution();

            _console.Trace($"Opening project file '{_options.ProjectFile}'");
            var project = await _workspace.OpenProjectAsync(_options.ProjectFile!);
            if (project == null)
            {
                _errorRepository.Add("Unable to open project file");
                return null;
            }

            return new ProjectFacade(project);
        }
    }
}
