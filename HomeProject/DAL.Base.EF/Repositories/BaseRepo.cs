﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Mappers;
using Contracts.DAL.Base.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public virtual TDALEntity Add(TDALEntity entity)
        {
            return Mapper.Map(RepoDbSet.Add(Mapper.MapReverse(entity)).Entity);
        }

        public virtual async Task<TDALEntity> UpdateAsync(TDALEntity entity)
        {
            var trackEntity = RepoDbSet.Find(entity.Id);
            var newEntity = Mapper.MapReverse(entity);

            RepoDbContext.Entry(trackEntity).State = EntityState.Detached;

//            return Mapper.Map(RepoDbSet.Update(Mapper.MapReverse(entity)).Entity);

            return Mapper.Map(RepoDbSet.Update(newEntity).Entity);
        }

        public virtual TDALEntity Remove(TDALEntity entity)
        {
            return Mapper.Map(RepoDbSet.Remove(Mapper.MapReverse(entity)).Entity);
        }

        public virtual TDALEntity Remove(Guid id)
        {
            return Mapper.Map(RepoDbSet.Remove(RepoDbSet.Find(id)).Entity);
        }

        public async Task<bool> CanAccess(Guid id, Guid userId)
        {
            throw new NotImplementedException();
//            try
//            {
//                var result = await RepoDbSet.FirstOrDefaultAsync(e => Microsoft.EntityFrameworkCore.EF
//                                                               .Property<Guid>(e, "profileId")
//                                                               .Equals(userId)
//                                                           && e.Id == id);
//
//                if (result != null)
//                {
//                    return true;
//                }
//            }
//            catch (Exception e)
//            {
//                return false;
//            }
//
//            return false;
        }
    }
}