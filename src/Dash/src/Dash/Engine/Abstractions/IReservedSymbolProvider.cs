namespace Dash.Engine.Abstractions
{
    public interface IReservedSymbolProvider
    {
        bool IsReservedEntityName(string keyword);
    }
}