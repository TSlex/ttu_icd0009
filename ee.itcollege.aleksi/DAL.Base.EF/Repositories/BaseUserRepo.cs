using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ee.itcollege.aleksi.Contracts.DAL.Base;
using ee.itcollege.aleksi.Contracts.DAL.Base.Mappers;
using ee.itcollege.aleksi.Contracts.DAL.Base.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ee.itcollege.aleksi.DAL.Base.EF.Repositories
{
    public class BaseUserRepo<TDomainEntity, TDALEntity, TDbContext, TUser> : IBaseDomainRepo<TDomainEntity, TDALEntity>
        where TDomainEntity : class, IDomainEntityBase<Guid>, new()
        where TDALEntity : class, IDomainEntityBase<Guid>, new()
        where TUser : IdentityUser<Guid>
        where TDbContext : DbContext
    {
        protected readonly TDbContext RepoDbContext;
        protected readonly DbSet<TDomainEntity> RepoDbSet;
        protected IBaseDALMapper<TDomainEntity, TDALEntity> Mapper;
        
        public BaseUserRepo(TDbContext dbContext, IBaseDALMapper<TDomainEntity, TDALEntity> mapper)
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
        
        public IEnumerable<TDALEntity> All()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TDALEntity>> AllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TDALEntity>> AllAdminAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TDALEntity>> GetRecordHistoryAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public TDALEntity Find(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TDALEntity> FindAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TDALEntity> FindAdminAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TDALEntity> GetForUpdateAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public TDALEntity Add(TDALEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TDALEntity> UpdateAsync(TDALEntity entity)
        {
            throw new NotImplementedException();
        }

        public TDALEntity Remove(TDALEntity tEntity)
        {
            throw new NotImplementedException();
        }

        public TDALEntity Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsUserAsync(Guid id, Guid? userId)
        {
            throw new NotImplementedException();
        }

        public void Restore(TDALEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TDALEntity> UpdateDomainAsync(TDomainEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}