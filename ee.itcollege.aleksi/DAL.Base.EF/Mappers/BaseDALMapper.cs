using AutoMapper;
using ee.itcollege.aleksi.Contracts.DAL.Base.Mappers;

namespace ee.itcollege.aleksi.DAL.Base.EF.Mappers
{
    public class BaseDALMapper<TInObject, TOutObject> : IBaseDALMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        private readonly IMapper _mapper;

        public BaseDALMapper()
        {
            _mapper = new MapperConfiguration(config => { config.AllowNullDestinationValues = true; }).CreateMapper();
        }

        public BaseDALMapper(IMapper mapper)
        {
            _mapper = mapper;
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