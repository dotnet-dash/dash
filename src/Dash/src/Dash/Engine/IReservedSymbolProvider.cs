namespace Dash.Engine
{
    public interface IReservedSymbolProvider
    {
        bool IsReservedEntityName(string keyword);
    }
}