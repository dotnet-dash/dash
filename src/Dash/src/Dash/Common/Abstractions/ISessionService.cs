using System.Threading.Tasks;

namespace Dash.Common.Abstractions
{
    public interface ISessionService
    {
        string GetTempPath(string fileName);
    }
}