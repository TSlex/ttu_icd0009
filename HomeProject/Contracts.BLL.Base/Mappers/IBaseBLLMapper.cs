namespace Contracts.BLL.Base.Mappers
{
    public interface IBaseBLLMapper<TInObject, TOutObject>
        where TOutObject : class, new()
        where TInObject : class, new()
    {
        TOutObject Map(TInObject inObject);
        TInObject MapReverse(TOutObject inObject);
    }
}
