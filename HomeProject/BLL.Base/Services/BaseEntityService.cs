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
        protected readonly IBaseBLLMapper<TDALEntity, TBLLEntity> Mapper;
        protected readonly TServiceRepository ServiceRepository;

        public BaseEntityService(TServiceRepository serviceRepository, IBaseBLLMapper<TDALEntity, TBLLEntity> mapper)
        {
            ServiceRepository = serviceRepository;
            Mapper = mapper;    
        }

        public virtual IEnumerable<TBLLEntity> All() =>
            ServiceRepository.All().Select(entity => Mapper.Map(entity));

        public virtual async Task<IEnumerable<TBLLEntity>> AllAsync() =>
            (await ServiceRepository.AllAsync()).Select(entity => Mapper.Map(entity));

        public virtual TBLLEntity Find(Guid id) =>
            Mapper.Map(ServiceRepository.Find(id));

        public virtual async Task<TBLLEntity> FindAsync(Guid id) =>
            Mapper.Map(await ServiceRepository.FindAsync(id));

        public virtual TBLLEntity Add(TBLLEntity entity) =>
            Mapper.Map(ServiceRepository.Add(Mapper.MapReverse(entity)));

        public virtual async Task<TBLLEntity> UpdateAsync(TBLLEntity entity) =>
            Mapper.Map(await ServiceRepository.UpdateAsync(Mapper.MapReverse(entity)));

        public virtual TBLLEntity Remove(TBLLEntity entity) =>
            Mapper.Map( ServiceRepository.Remove(Mapper.MapReverse(entity)));

        public virtual TBLLEntity Remove(Guid id) =>
            Mapper.Map( ServiceRepository.Remove(id));

        public virtual async Task<IEnumerable<TBLLEntity>> AllByPageAsync(int pageNumber, int count, bool order = false, bool reversed = false, string orderProperty = null)
        {
            return (await ServiceRepository.AllByPageAsync(pageNumber, count, order, reversed, orderProperty))
                .Select(entity => Mapper.Map(entity));
        }
        
        public virtual async Task<IEnumerable<TBLLEntity>> AllByIdPageAsync(int pageNumber, int count, string filterProperty, Guid id, bool order = false, bool reversed = false, string orderProperty = null)
        {
            return (await ServiceRepository.AllByIdPageAsync(pageNumber, count, filterProperty, id, order, reversed, orderProperty))
                .Select(entity => Mapper.Map(entity));
        }
        
        public virtual async Task<int> CountByIdAsync(string filterProperty, Guid id)
        {
            return await ServiceRepository.CountByIdAsync(filterProperty, id);
        }

        public virtual async Task<int> CountAsync()
        {
            return await ServiceRepository.CountAsync();
        }
    }
}