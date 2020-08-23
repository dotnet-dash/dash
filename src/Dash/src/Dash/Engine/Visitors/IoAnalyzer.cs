using System.IO.Abstractions;
using System.Threading.Tasks;
using Dash.Engine.Abstractions;
using Dash.Extensions;
using Dash.Nodes;

namespace Dash.Engine.Visitors
{
    public class IoAnalyzer : BaseVisitor
    {
        private readonly IFileSystem _fileSystem;
        private readonly IErrorRepository _errorRepository;

        public IoAnalyzer(IConsole console, IFileSystem fileSystem, IErrorRepository errorRepository) : base(console)
        {
            _fileSystem = fileSystem;
            _errorRepository = errorRepository;
        }

        public override Task Visit(UriNode node)
        {
            if (node.Uri.Scheme.IsSame("file") &&
                node.UriMustExist &&
                !_fileSystem.File.Exists(node.Uri.AbsolutePath))
            {
                _errorRepository.Add($"File does not exist: '{node.Uri}'");
            }

            return base.Visit(node);
        }
    }
}
