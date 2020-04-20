using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.Base.Mappers;
using Contracts.BLL.Base.Services;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Repositories;

namespace BLL.Base.Services
{
    public class BaseEntityService<TServiceRepository, TDALEntity, TBLLEntity> : BaseService,
        IBaseEntityService<TDALEntity, TBLLEntity>
        where TBLLEntity : class, IDomainEntityBaseMetadata<Guid>, new()
        where TDALEntity : class, IDomainEntityBaseMetadata<Guid>, new()
        where TServiceRepository : IBaseRepo<TDALEntity>
    {
        private readonly IBaseBLLMapper<TDALEntity, TBLLEntity> _mapper;
        protected readonly TServiceRepository ServiceRepository;

        public BaseEntityService(TServiceRepository serviceRepository, IBaseBLLMapper<TDALEntity, TBLLEntity> mapper)
        {
            ServiceRepository = serviceRepository;
            _mapper = mapper;    
        }

        public virtual IEnumerable<TBLLEntity> All() =>
            ServiceRepository.All().Select(entity => _mapper.Map(entity));

        public virtual async Task<IEnumerable<TBLLEntity>> AllAsync() =>
            (await ServiceRepository.AllAsync()).Select(entity => _mapper.Map(entity));

        public virtual TBLLEntity Find(params object[] id) =>
            _mapper.Map(ServiceRepository.Find(id));

        public virtual async Task<TBLLEntity> FindAsync(params object[] id) =>
            _mapper.Map(await ServiceRepository.FindAsync(id));

        public virtual TBLLEntity Add(TBLLEntity entity) =>
            _mapper.Map(ServiceRepository.Add(_mapper.MapReverse(entity)));

        public virtual TBLLEntity Update(TBLLEntity entity) =>
            _mapper.Map(ServiceRepository.Update(_mapper.MapReverse(entity)));

        public virtual TBLLEntity Remove(TBLLEntity entity) =>
            _mapper.Map(ServiceRepository.Remove(_mapper.MapReverse(entity)));

        public virtual TBLLEntity Remove(params object[] id) =>
            _mapper.Map(ServiceRepository.Remove(id));
    }
}