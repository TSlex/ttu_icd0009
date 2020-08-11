using AutoMapper;
using BLL.App.DTO;
using ee.itcollege.aleksi.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class UniversalBLLMapper<TInObject, TOutObject> : BaseBLLMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        public UniversalBLLMapper() : base(
            new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();

                // own mapping for composite entities
                config.CreateMap<DAL.App.DTO.Template, Template>();
                config.CreateMap<Template, DAL.App.DTO.Template>();

                config.AllowNullDestinationValues = true;
            }).CreateMapper())
        {
        }
    }
}