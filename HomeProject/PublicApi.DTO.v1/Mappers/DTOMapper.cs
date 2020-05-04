using AutoMapper;
using DAL.Base.EF.Mappers;

namespace PublicApi.DTO.v1.Mappers
{
    public class DTOMapper<TInObject, TOutObject>: BaseDALMapper<TInObject, TOutObject> 
        where TOutObject : class, new() 
        where TInObject : class, new()
    {
        public DTOMapper() : base(null)
        {
        }
    }
}