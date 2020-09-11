using System.Threading.Tasks;

namespace Dash.Roslyn
{
    public interface IWorkspace
    {
        Task<IProject?> OpenProjectAsync();
    }
}
