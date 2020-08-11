using AutoMapper;
using ee.itcollege.aleksi.BLL.Base.Mappers;

namespace PublicApi.v1.Mappers
{
    public class UniversalAPIMapper<TInObject, TOutObject> : BaseBLLMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        public UniversalAPIMapper() : base(
            new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();

                // own mapping for composite entities
                config.CreateMap<BLL.App.DTO.Template, Template>();
                config.CreateMap<Template, BLL.App.DTO.Template>();

                config.AllowNullDestinationValues = true;
            }).CreateMapper())
        {
        }
    }
}