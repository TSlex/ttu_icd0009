namespace Contracts.BLL.Base.Mappers
{
    public interface IBaseBLLMapper
    {
        TOutObject Map<TInObject, TOutObject>(TInObject inObject)
            where TOutObject : class, new()
            where TInObject : class, new();
    }
}