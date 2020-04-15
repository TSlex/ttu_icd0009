using System;
using Contracts.BLL.Base.Mappers;

namespace BLL.Base.Mappers
{
    public class IdentityMapper : IBaseBLLMapper
    {
        public TOutObject Map<TInObject, TOutObject>(TInObject inObject)
            where TInObject : class, new()
            where TOutObject : class, new()
        {
            return (inObject as TOutObject)!;
        }
    }
}