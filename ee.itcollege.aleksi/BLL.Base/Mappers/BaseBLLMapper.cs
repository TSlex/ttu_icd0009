using AutoMapper;
using ee.itcollege.aleksi.Contracts.BLL.Base.Mappers;

namespace ee.itcollege.aleksi.BLL.Base.Mappers
{
    public class BaseBLLMapper<TInObject, TOutObject> : IBaseBLLMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        private readonly IMapper _mapper;

        public BaseBLLMapper()
        {
            _mapper = new MapperConfiguration(config => { config.AllowNullDestinationValues = true; }).CreateMapper();
        }

        public BaseBLLMapper(IMapper mapper)
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