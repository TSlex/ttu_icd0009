using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contracts.DAL.Base.Repositories
{
    public interface IBaseRepo<TEntity> : IBaseRepo<TEntity, Guid>
        where TEntity : class, IDomainEntity<Guid>, new()
    {
    }

    public interface IBaseRepo<TEntity, TKey>
        where TEntity : class, IDomainEntity<TKey>, new()
        where TKey : struct, IComparable
    {
        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync();
        
        IEnumerable<TEntity> All(params Expression<Func<TEntity, object>>[]
            includeProperties);
        
        Task<IEnumerable<TEntity>> AllAsync(params Expression<Func<TEntity, object>>[]
            includeProperties);

        TEntity Find(params object[] id);
        Task<TEntity> FindAsync(params object[] id);

        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);

        TEntity Remove(TEntity entity);
        TEntity Remove(params object[] id);
    }
}