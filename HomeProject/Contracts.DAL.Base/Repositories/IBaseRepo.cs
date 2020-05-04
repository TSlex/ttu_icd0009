using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contracts.DAL.Base.Repositories
{
    public interface IBaseRepo<TDALEntity> : IBaseRepo<TDALEntity, Guid>
        where TDALEntity : class, IDomainEntityBase<Guid>, new()
    {
    }

    public interface IBaseRepo<TDALEntity, TKey>
        where TDALEntity : class, IDomainEntityBase<Guid>, new()
        where TKey : struct, IEquatable<TKey>
    {
        IEnumerable<TDALEntity> All();
        Task<IEnumerable<TDALEntity>> AllAsync();

        TDALEntity Find(TKey id);
        Task<TDALEntity> FindAsync(TKey id);

        TDALEntity Add(TDALEntity entity);

        Task<TDALEntity> UpdateAsync(TDALEntity entity);

        TDALEntity Remove(TDALEntity entity);
        TDALEntity Remove(TKey id);

        Task<IEnumerable<TDALEntity>> AllByPageAsync(int pageNumber, int count, bool order = false,
            bool reversed = false, string? orderProperty = null);
        
        Task<IEnumerable<TDALEntity>> AllByIdPageAsync(int pageNumber, int count, string filterProperty, Guid id,
            bool order = false, bool reversed = false, string orderProperty = null);

        Task<int> CountByIdAsync(string filterProperty, Guid id);

        Task<int> CountAsync();
    }
}