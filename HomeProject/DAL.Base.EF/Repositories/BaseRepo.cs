using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Mappers;
using Contracts.DAL.Base.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DAL.Base.EF.Repositories
{
    public class BaseRepo<TDomainEntity, TDALEntity, TDbContext> : IBaseRepo<TDALEntity>
        where TDomainEntity : class, IDomainEntityBase<Guid>, new()
        where TDALEntity : class, IDomainEntityBase<Guid>, new()
        where TDbContext : DbContext
    {
        protected readonly TDbContext RepoDbContext;
        protected readonly DbSet<TDomainEntity> RepoDbSet;
        protected IBaseDALMapper<TDomainEntity, TDALEntity> Mapper;

        public BaseRepo(TDbContext dbContext, IBaseDALMapper<TDomainEntity, TDALEntity> mapper)
        {
            RepoDbContext = dbContext;
            RepoDbSet = RepoDbContext.Set<TDomainEntity>();
            Mapper = mapper;

            if (RepoDbSet == null)
            {
                throw new ArgumentNullException(typeof(TDomainEntity).Name + " was not found as DBSet!");
            }

            RepoDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public virtual IEnumerable<TDALEntity> All()
        {
            return RepoDbSet.ToList().Select(entity => Mapper.Map(entity));
        }

        public virtual async Task<IEnumerable<TDALEntity>> AllAsync()
        {
            return (await RepoDbSet.ToListAsync()).Select(entity => Mapper.Map(entity));
        }

        public virtual TDALEntity Find(Guid id)
        {
            return Mapper.Map(RepoDbSet.Find(id));
        }

        public virtual async Task<TDALEntity> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet.FindAsync(id));
        }
        
        public async Task<TDALEntity> GetForUpdateAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet.FindAsync(id));
        }

        public virtual TDALEntity Add(TDALEntity entity)
        {
            return Mapper.Map(RepoDbSet.Add(Mapper.MapReverse(entity)).Entity);
        }

        #pragma warning disable 1998
        public virtual async Task<TDALEntity> UpdateAsync(TDALEntity entity)
        #pragma warning restore 1998
        {
            var trackEntity = RepoDbSet.Find(entity.Id);
            var newEntity = Mapper.MapReverse(entity);

            RepoDbContext.Entry(trackEntity).State = EntityState.Detached;

//            return Mapper.Map(RepoDbSet.Update(Mapper.MapReverse(entity)).Entity);

            return Mapper.Map(RepoDbSet.Update(newEntity).Entity);
        }

        public virtual TDALEntity Remove(TDALEntity entity)
        {
            var trackEntity = RepoDbSet.Find(entity.Id);
            var newEntity = Mapper.MapReverse(entity);

            RepoDbContext.Entry(trackEntity).State = EntityState.Detached;

            return Mapper.Map(RepoDbSet.Remove(newEntity).Entity);
        }

        public virtual TDALEntity Remove(Guid id)
        {
            return Mapper.Map(RepoDbSet.Remove(RepoDbSet.Find(id)).Entity);
        }

        public virtual async Task<int> CountAsync()
        {
            return await RepoDbSet.CountAsync();
        }

        public async Task<bool> Exist(Guid id)
        {
            return (await RepoDbSet.FirstOrDefaultAsync(x => x.Id == id)) != null;
        }
    }
}