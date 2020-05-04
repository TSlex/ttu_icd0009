using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Repositories;

namespace Contracts.BLL.Base.Services
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

        TBLLEntity Find(Guid id);
        Task<TBLLEntity> FindAsync(Guid id);

        TBLLEntity Add(TBLLEntity entity);
        Task<TBLLEntity> UpdateAsync(TBLLEntity entity);

        TBLLEntity Remove(TBLLEntity entity);
        TBLLEntity Remove(Guid id);

        Task<IEnumerable<TBLLEntity>> AllByPageAsync(int pageNumber, int count, bool order = false,
            bool reversed = false, string? orderProperty = null);

        Task<IEnumerable<TBLLEntity>> AllByIdPageAsync(int pageNumber, int count, string filterProperty, Guid id,
            bool order = false, bool reversed = false, string orderProperty = null);

        Task<int> CountByIdAsync(string filterProperty, Guid id);

        Task<int> CountAsync();
    }
}