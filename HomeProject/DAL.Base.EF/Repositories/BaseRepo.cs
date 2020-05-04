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
            var trackEntity = RepoDbSet.Find(entity.Id);
            var newEntity = Mapper.MapReverse(entity);

            RepoDbContext.Entry(trackEntity).State = EntityState.Detached;

            return Mapper.Map(RepoDbSet.Remove(newEntity).Entity);
        }

        public virtual TDALEntity Remove(Guid id)
        {
            return Mapper.Map(RepoDbSet.Remove(RepoDbSet.Find(id)).Entity);
        }

        public virtual async Task<IEnumerable<TDALEntity>> AllByPageAsync(int pageNumber, int count, bool order = false,
            bool reversed = false, string? orderProperty = null)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * count;

            if (pageIndex < 0)
            {
                return new TDALEntity[] { };
            }

            var query = RepoDbSet.AsQueryable();

            if (order && orderProperty != null)
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(TDALEntity)).Find(orderProperty, true);

                if (!reversed)
                {
                    query = RepoDbSet.OrderByDescending(x => prop.GetValue(x));
                }
                else
                {
                    query = RepoDbSet.OrderBy(x => prop.GetValue(x));
                }
            }

            return (await query.Skip(startIndex).Take(count).ToListAsync()).Select(x => Mapper.Map(x));
        }

        public virtual async Task<IEnumerable<TDALEntity>> AllByIdPageAsync(int pageNumber, int count,
            string filterProperty, Guid id, bool order = false,
            bool reversed = false, string orderProperty = null)
        {
            var filterProp = TypeDescriptor.GetProperties(typeof(TDALEntity)).Find(filterProperty, true);

            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * count;

            if (pageIndex < 0)
            {
                return new TDALEntity[] { };
            }

            var query = RepoDbSet.Where(x => filterProp.GetValue(x).ToString() == id.ToString()).AsQueryable();

            if (order && orderProperty != null)
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(TDALEntity)).Find(orderProperty, true);

                if (!reversed)
                {
                    query = RepoDbSet.OrderByDescending(x => prop.GetValue(x));
                }
                else
                {
                    query = RepoDbSet.OrderBy(x => prop.GetValue(x));
                }
            }

            return (await query.Skip(startIndex).Take(count).ToListAsync()).Select(x => Mapper.Map(x));
        }

        public virtual async Task<int> CountByIdAsync(string filterProperty, Guid id)
        {
//            var prop = TypeDescriptor.GetProperties(typeof(TDALEntity)).Find(filterProperty, true);

//            var prop = typeof(TDALEntity).GetProperty(filterProperty);

//            return (await RepoDbSet
//                .Select(item => new {value = item, prop = (string) prop.GetValue(item)})
//                .Where(item => string.Equals(id.ToString(), item.prop)).ToListAsync()).Count;

//            var query = RepoDbSet.AsQueryable().Where(x => (Guid) prop.GetValue(x) == id);

//            var all = await RepoDbSet.ToListAsync();

//            var query = RepoDbSet.AsEnumerable().Select(item => new {value = item, lol = prop.GetValue(item)}).ToList();
//            var query = all.Select(e => prop.GetValue(e, null));


            var query = RepoDbSet.First();

            object value = query.GetType().GetProperty(filterProperty).GetValue(query, null);

            var test = RepoDbSet.Select(e => e.GetType().GetProperty(filterProperty).GetValue(e, null));

            var test2 = RepoDbSet.Where(e => e.GetType().GetProperty(filterProperty).GetValue(e, null) != null);

//            return await RepoDbSet.CountAsync(e => (Guid)e.GetType().GetProperty(filterProperty).GetValue(e, null) == id);

//            var test3 = RepoDbContext.Set<TDomainEntity>().Where("propertyName == @0", id);

            var hui = (RepoDbContext as DbContext).Set<Favorite>().S
            
            return 1;
        }

        public virtual async Task<int> CountAsync()
        {
            return await RepoDbSet.CountAsync();
        }
    }
}