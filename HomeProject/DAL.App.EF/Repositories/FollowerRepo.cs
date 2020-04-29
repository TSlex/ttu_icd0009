using System;
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
    }
}