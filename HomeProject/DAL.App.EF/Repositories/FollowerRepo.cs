using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class FollowerRepo : BaseRepo<Domain.Follower, Follower, ApplicationDbContext>, IFollowerRepo
    {
        public FollowerRepo(ApplicationDbContext dbContext) :
            base(dbContext, new FollowerMapper())
        {
        }

        public async Task<Follower> FindAsync(Guid userId, Guid profileId)
        {
            return Mapper.Map(await RepoDbSet.FirstOrDefaultAsync(sub =>
                sub.FollowerProfileId == userId && sub.ProfileId == profileId));
        }

        public async Task<int> CountByIdAsync(Guid userId, bool reversed)
        {
            if (reversed)
            {
                return await RepoDbContext.Followers.CountAsync(follower => follower.FollowerProfileId == userId);
            }

            return await RepoDbContext.Followers.CountAsync(follower => follower.ProfileId == userId);
        }

        public async Task<IEnumerable<Follower>> AllByIdPageAsync(Guid userId, bool reversed, int pageNumber, int count)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * count;

            if (pageIndex < 0)
            {
                return new Follower[] { };
            }

            if (reversed)
            {
                return (await GetQuery()
                        .Where(follower => follower.FollowerProfileId == userId)
                        .Skip(startIndex)
                        .Take(count)
                        .ToListAsync())
                    .Select(post => Mapper.Map(post));
            }

            return (await GetQuery()
                    .Where(follower => follower.ProfileId == userId)
                    .Skip(startIndex)
                    .Take(count)
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }

        private IQueryable<Domain.Follower> GetQuery()
        {
            return RepoDbSet
                .Include(follower => follower.Profile)
                .Include(follower => follower.FollowerProfile)
                .AsQueryable();
        }
    }
}