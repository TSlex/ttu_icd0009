using System;
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
    }
}