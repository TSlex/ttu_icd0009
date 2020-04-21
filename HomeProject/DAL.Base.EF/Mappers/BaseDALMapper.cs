using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.Base.EF.Mappers
{
    public class BaseDALMapper<TInObject, TOutObject> : IBaseDALMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        private readonly IMapper _mapper;

        public BaseDALMapper(): this(null)
        {
            
        }
        
        public BaseDALMapper(IMapper? mapper)
        {
            _mapper = mapper ?? new MapperConfiguration(config =>
            {
                config.CreateMap<TInObject, TOutObject>();
                config.CreateMap<TOutObject, TInObject>();
                config.AllowNullDestinationValues = true;
                
            }).CreateMapper();
        }

        public virtual TOutObject Map(TInObject inObject)
        {
            return _mapper.Map<TInObject, TOutObject>(inObject);
        }

        public virtual TInObject MapReverse(TOutObject outObject)
        {
            return _mapper.Map<TOutObject, TInObject>(outObject);
        }
    }
}