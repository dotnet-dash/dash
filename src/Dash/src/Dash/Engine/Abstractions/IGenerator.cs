using System.Threading.Tasks;
using Dash.Engine.Models.SourceCode;

namespace Dash.Engine.Abstractions
{
    public interface IGenerator
    {
        Task Generate(SourceCodeDocument model);
    }
}
