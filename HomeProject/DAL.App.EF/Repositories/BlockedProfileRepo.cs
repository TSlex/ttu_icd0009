using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BlockedProfileRepo : BaseRepo<BlockedProfile, ApplicationDbContext>, IBlockedProfileRepo
    {
        public BlockedProfileRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}