using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Repositories;
using DAL.Mappers;

namespace DAL.Repositories
{
    public class BlockedProfileRepo : BaseRepo<Domain.BlockedProfile, BlockedProfile, ApplicationDbContext>, IBlockedProfileRepo
    {
        public BlockedProfileRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new BlockedProfileMapper())
        {
        }
    }
}