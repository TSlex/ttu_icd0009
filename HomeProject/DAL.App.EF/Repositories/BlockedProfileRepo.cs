using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BlockedProfileRepo : BaseRepo<BlockedProfile>, IBlockedProfileRepo
    {
        public BlockedProfileRepo(DbContext dbContext) : base(dbContext)
        {
        }
    }
}