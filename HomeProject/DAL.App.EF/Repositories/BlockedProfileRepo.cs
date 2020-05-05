using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BlockedProfileRepo : BaseRepo<Domain.BlockedProfile, BlockedProfile, ApplicationDbContext>,
        IBlockedProfileRepo
    {
        public BlockedProfileRepo(ApplicationDbContext dbContext) :
            base(dbContext, new BlockedProfileMapper())
        {
        }

        public async Task<BlockedProfile> FindAsync(Guid userId, Guid profileId)
        {
            return Mapper.Map(await RepoDbSet.FirstOrDefaultAsync(prop =>
                prop.ProfileId == userId && prop.BProfileId == profileId));
        }

        public async Task<int> CountByIdAsync(Guid userId)
        {
            return await RepoDbContext.BlockedProfiles.CountAsync(profile => profile.ProfileId == userId);
        }

        public async Task<IEnumerable<BlockedProfile>> AllByIdPageAsync(Guid userId, int pageNumber, int count)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * count;

            if (pageIndex < 0)
            {
                return new BlockedProfile[] { };
            }

            return (await RepoDbContext.BlockedProfiles
                    .Where(profile => profile.ProfileId == userId)
                    .Include(profile => profile.BProfile)
                    .Skip(startIndex)
                    .Take(count)
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }
    }
}