using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contracts.DAL.Base.Repositories
{
    public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, string>
        where TEntity : class, IDomainEntity, new()
    {
    }

    public interface IBaseRepository<TEntity, TKey>
        where TEntity : class, IDomainEntity<TKey>, new()
        where TKey : IComparable
    {

        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync();
        
//        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? predicate = null);
//        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null);
        
        TEntity Find(params object[] id);
        Task<TEntity> FindAsync(params object[] id);

        TEntity Add(TEntity entity);
        
        TEntity Update(TEntity entity);
        
        TEntity Remove(TEntity entity);
        
        TEntity Remove(params object[] id);
        
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}