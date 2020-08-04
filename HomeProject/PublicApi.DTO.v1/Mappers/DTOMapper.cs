using AutoMapper;
using ee.itcollege.aleksi.DAL.Base.EF.Mappers;

namespace PublicApi.DTO.v1.Mappers
{
    public class DTOMapper<TInObject, TOutObject>: BaseDALMapper<TInObject, TOutObject> 
        where TOutObject : class, new() 
        where TInObject : class, new()
    {
    }
}