namespace Contracts.DAL.Base.Mappers
{
    public interface IBaseDALMapper<TInObject, TOutObject>
        where TOutObject : class, new()
        where TInObject : class, new()
    {
        TOutObject Map(TInObject inObject);
        TInObject MapReverse(TOutObject inObject);
    }
}