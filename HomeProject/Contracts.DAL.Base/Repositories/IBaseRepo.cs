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
        Task<IEnumerable<TDALEntity>> AllAdminAsync();

        Task<IEnumerable<TDALEntity>> GetRecordHistoryAsync(Guid id);

        TDALEntity Find(TKey id);
        Task<TDALEntity> FindAsync(TKey id);
        Task<TDALEntity> FindAdminAsync(TKey id);
        Task<TDALEntity> GetForUpdateAsync(Guid id);

        TDALEntity Add(TDALEntity entity);

        Task<TDALEntity> UpdateAsync(TDALEntity entity);

        TDALEntity Remove(TDALEntity tEntity);
        TDALEntity Remove(TKey id);

        Task<int> CountAsync();
        Task<bool> ExistAsync(Guid id);

        void Restore(TDALEntity entity);
    }
    
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IBaseDomainRepo<TDomainEntity, TDALEntity> : IBaseRepo<TDALEntity, Guid>
        where TDomainEntity : class, IDomainEntityBase<Guid>, new()
        where TDALEntity : class, IDomainEntityBase<Guid>, new()
    {
        Task<TDALEntity> UpdateDomainAsync(TDomainEntity entity);
    }
}