using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contracts.DAL.Base.Repositories
{
    public interface IBaseRepo<TDALEntity> : IBaseRepo<TDALEntity, Guid>
        where TDALEntity: class, IDomainEntityBase<Guid>, new()
    {
    }

    public interface IBaseRepo<TDALEntity, TKey>
        where TDALEntity: class, IDomainEntityBase<Guid>, new()
        where TKey : struct, IComparable
    {
        IEnumerable<TDALEntity> All();
        Task<IEnumerable<TDALEntity>> AllAsync();

        TDALEntity Find(params object[] id);
        Task<TDALEntity> FindAsync(params object[] id);

        TDALEntity Add(TDALEntity entity);
        TDALEntity Update(TDALEntity entity);

        TDALEntity Remove(TDALEntity entity);
        TDALEntity Remove(params object[] id);    
    }
}