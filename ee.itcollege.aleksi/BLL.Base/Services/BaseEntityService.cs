using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ee.itcollege.aleksi.Contracts.BLL.Base.Mappers;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;
using ee.itcollege.aleksi.Contracts.DAL.Base;
using ee.itcollege.aleksi.Contracts.DAL.Base.Repositories;

namespace ee.itcollege.aleksi.BLL.Base.Services
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

        public async Task<IEnumerable<TBLLEntity>> AllAdminAsync() =>
            (await ServiceRepository.AllAdminAsync())
            .OrderByDescending(entity => entity.CreatedAt)
            .Select(entity => Mapper.Map(entity));

        public async Task<IEnumerable<TBLLEntity>> GetRecordHistoryAsync(Guid id) =>
            (await ServiceRepository.GetRecordHistoryAsync(id))
            .OrderByDescending(entity => entity.CreatedAt)
            .Select(entity => Mapper.Map(entity));

        public virtual TBLLEntity Find(Guid id) =>
            Mapper.Map(ServiceRepository.Find(id));

        public virtual async Task<TBLLEntity> FindAsync(Guid id) =>
            Mapper.Map(await ServiceRepository.FindAsync(id));

        public virtual async Task<TBLLEntity> FindAdminAsync(Guid id) =>
            Mapper.Map(await ServiceRepository.FindAdminAsync(id));

        public async Task<TBLLEntity> GetForUpdateAsync(Guid id) =>
            Mapper.Map(await ServiceRepository.GetForUpdateAsync(id));

        public virtual TBLLEntity Add(TBLLEntity entity) =>
            Mapper.Map(ServiceRepository.Add(Mapper.MapReverse(entity)));

        public virtual async Task<TBLLEntity> UpdateAsync(TBLLEntity entity) =>
            Mapper.Map(await ServiceRepository.UpdateAsync(Mapper.MapReverse(entity)));

        public virtual TBLLEntity Remove(TBLLEntity entity) =>
            Mapper.Map(ServiceRepository.Remove(Mapper.MapReverse(entity)));

        public virtual TBLLEntity Remove(Guid id) =>
            Mapper.Map(ServiceRepository.Remove(id));

        public virtual void Restore(TBLLEntity entity)
            => ServiceRepository.Restore(Mapper.MapReverse(entity));

        public virtual async Task<int> CountAsync()
        {
            return await ServiceRepository.CountAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await ServiceRepository.ExistAsync(id);
        }
    }
}