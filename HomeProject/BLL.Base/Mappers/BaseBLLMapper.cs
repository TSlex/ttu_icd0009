using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.Base.Mappers
{
    public class BaseBLLMapper<TInObject, TOutObject> : IBaseBLLMapper
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        private readonly IMapper _mapper;

        public BaseBLLMapper()
        {
            _mapper = new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();
            }).CreateMapper();
        }

        #pragma warning disable 693
        public TOutObject Map<TInObject, TOutObject>(TInObject inObject)
        #pragma warning restore 693
            where TInObject : class, new()
            where TOutObject : class, new()
        {
            return _mapper.Map<TInObject, TOutObject>(inObject);
        }
    }
}