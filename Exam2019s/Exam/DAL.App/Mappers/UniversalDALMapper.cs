using AutoMapper;
using DAL.App.DTO;
using ee.itcollege.aleksi.DAL.Base.EF.Mappers;

namespace DAL.App.Mappers
{
    public class UniversalDALMapper<TInObject, TOutObject> : BaseDALMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        public UniversalDALMapper() : base(
            new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();
                
                // own mapping for composite entities


                config.AllowNullDestinationValues = true;
            }).CreateMapper())
        {
        }
    }
}