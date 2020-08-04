using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ee.itcollege.aleksi.Contracts.DAL.Base;

namespace ee.itcollege.aleksi.Contracts.BLL.Base.Services
{
    public interface IBaseService
    {
        // add common base methods here
    }

    public interface IBaseEntityService<TDALEntity, TBLLEntity> : IBaseService
        where TDALEntity : class, IDomainEntityBase<Guid>, new()
        where TBLLEntity : class, IDomainEntityBase<Guid>, new()
    {
        IEnumerable<TBLLEntity> All();
        Task<IEnumerable<TBLLEntity>> AllAsync();
        Task<IEnumerable<TBLLEntity>> AllAdminAsync();
        
        Task<IEnumerable<TBLLEntity>> GetRecordHistoryAsync(Guid id);

        TBLLEntity Find(Guid id);
        Task<TBLLEntity> FindAsync(Guid id);
        Task<TBLLEntity> FindAdminAsync(Guid id);
        
        Task<TBLLEntity> GetForUpdateAsync(Guid id);

        TBLLEntity Add(TBLLEntity entity);
        Task<TBLLEntity> UpdateAsync(TBLLEntity entity);

        TBLLEntity Remove(TBLLEntity entity);
        TBLLEntity Remove(Guid id);

        void Restore(TBLLEntity entity);

        Task<int> CountAsync();

        Task<bool> ExistsAsync(Guid id);
    }
}