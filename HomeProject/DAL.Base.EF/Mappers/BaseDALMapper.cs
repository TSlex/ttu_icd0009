using AutoMapper;
using Contracts.DAL.Base.Mappers;
using DAL.App.DTO;
using Profile = DAL.App.DTO.Profile;

namespace DAL.Base.EF.Mappers
{
    public class BaseDALMapper<TInObject, TOutObject> : IBaseDALMapper<TInObject, TOutObject>
        where TInObject : class, new()
        where TOutObject : class, new()
    {
        private readonly IMapper _mapper;

        public BaseDALMapper() : this(null)
        {
        }

        public BaseDALMapper(IMapper? mapper)
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