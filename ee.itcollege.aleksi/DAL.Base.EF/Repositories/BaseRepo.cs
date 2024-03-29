﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ee.itcollege.aleksi.Contracts.DAL.Base;
using ee.itcollege.aleksi.Contracts.DAL.Base.Mappers;
using ee.itcollege.aleksi.Contracts.DAL.Base.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ee.itcollege.aleksi.DAL.Base.EF.Repositories
{
    public class BaseRepo<TDomainEntity, TDALEntity, TDbContext> : IBaseDomainRepo<TDomainEntity, TDALEntity>
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

        public virtual async Task<IEnumerable<TDALEntity>> AllAdminAsync()
        {
            return (await RepoDbSet.ToListAsync())
                .Where(FilterHistoryRows)
                .Select(entity => Mapper.Map(entity));
        }

        public virtual IEnumerable<TDALEntity> All()
        {
            return RepoDbSet.ToList()
                .Where(FilterDeletedRows)
                .Select(entity => Mapper.Map(entity));
        }

        public virtual async Task<IEnumerable<TDALEntity>> AllAsync()
        {
            return (await RepoDbSet.ToListAsync())
                .Where(FilterDeletedRows)
                .Select(entity => Mapper.Map(entity));
        }

        public virtual Task<IEnumerable<TDALEntity>> GetRecordHistoryAsync(Guid id)
        {
            throw new NotSupportedException();
        }

        private static bool FilterDeletedRows(TDomainEntity entity)
        {
            if (entity is IDomainEntityBaseMetadata baseMetadata)
            {
                return baseMetadata.DeletedAt == null;
            }

            return true;
        }
        
        private static bool FilterHistoryRows(TDomainEntity entity)
        {
            if (entity is ISoftUpdateEntity softUpdateEntity)
            {
                return softUpdateEntity.MasterId == null;
            }

            return true;
        }

        public virtual TDALEntity Find(Guid id)
        {
            return Mapper.Map(RepoDbSet.Find(id));
        }

        public virtual async Task<TDALEntity> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet.FindAsync(id));
        }
        
        public virtual async Task<TDALEntity> FindAdminAsync(Guid id)
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

            var now = DateTime.Now;

            if (newEntity is IDomainEntityBaseMetadata originBaseMetadata)
            {
                originBaseMetadata.CreatedAt = now;
            }

            RepoDbContext.Entry(trackEntity).State = EntityState.Detached;

            if (trackEntity is ISoftUpdateEntity softUpdateEntity &&
                trackEntity is IDomainEntityBaseMetadata baseMetadata)
            {
                softUpdateEntity.MasterId = newEntity.Id;
                
                baseMetadata.ChangedBy = "history";
                baseMetadata.ChangedAt = now;
                baseMetadata.DeletedBy = "history";
                baseMetadata.DeletedAt = now;
                baseMetadata.Id = Guid.NewGuid();

                RepoDbSet.Add(trackEntity);
            }

            return Mapper.Map(RepoDbSet.Update(newEntity).Entity);
        }
        
#pragma warning disable 1998
        public virtual async Task<TDALEntity> UpdateDomainAsync(TDomainEntity entity)
#pragma warning restore 1998
        {
            var trackEntity = RepoDbSet.Find(entity.Id);
            var newEntity = entity;

            var now = DateTime.Now;

            if (newEntity is IDomainEntityBaseMetadata originBaseMetadata)
            {
                originBaseMetadata.CreatedAt = now;
            }

            RepoDbContext.Entry(trackEntity).State = EntityState.Detached;

            if (trackEntity is ISoftUpdateEntity softUpdateEntity &&
                trackEntity is IDomainEntityBaseMetadata baseMetadata)
            {
                softUpdateEntity.MasterId = newEntity.Id;
                
                baseMetadata.ChangedBy = "history";
                baseMetadata.ChangedAt = now;
                baseMetadata.DeletedBy = "history";
                baseMetadata.DeletedAt = now;
                baseMetadata.Id = Guid.NewGuid();

                RepoDbSet.Add(trackEntity);
            }

            return Mapper.Map(RepoDbSet.Update(newEntity).Entity);
        }
        
        public virtual TDALEntity Remove(TDALEntity tEntity)
        {
            var trackEntity = RepoDbSet.Find(tEntity.Id);
            var newEntity = Mapper.MapReverse(tEntity);

            RepoDbContext.Entry(trackEntity).State = EntityState.Detached;

            return Mapper.Map(RepoDbSet.Remove(newEntity).Entity);
        }

        public virtual TDALEntity Remove(Guid id)
        {
            return Remove(Mapper.Map(RepoDbSet.Find(id)));
        }

        public virtual void Restore(TDALEntity entity)
        {
            if (!(entity is IDomainEntityMetadata entityMetadata)) return;

            entityMetadata.DeletedAt = null;
            entityMetadata.DeletedBy = null;

            RepoDbSet.Update(Mapper.MapReverse(entity));
        }

        public virtual async Task<int> CountAsync()
        {
            return await RepoDbSet.CountAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return (await RepoDbSet.FirstOrDefaultAsync(x => x.Id == id)) != null;
        }
        
        public Task<bool> ExistsUserAsync(Guid id, Guid? userId)
        {
            throw new NotImplementedException();
        }
    }
}