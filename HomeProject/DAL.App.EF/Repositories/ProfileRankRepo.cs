using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;
using Rank = Domain.Rank;

namespace DAL.Repositories
{
    public class ProfileRankRepo : BaseRepo<Domain.ProfileRank, ProfileRank, ApplicationDbContext>, IProfileRankRepo
    {
        public ProfileRankRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new ProfileRankMapper())
        {
        }

        public async Task<IEnumerable<ProfileRank>> AllUserAsync(Guid profileId)
        {
            return await RepoDbContext.ProfileRanks
                .Where(rank => rank.ProfileId == profileId)
                .Include(rank => rank.Rank)
                .Select(rank => Mapper.Map(rank)).ToListAsync();
        }

        public Task<ProfileRank> ActiveUserAsync(Guid profileId)
        {
            throw new NotImplementedException();
            /*return (await RepoDbContext.ProfileRanks
                .Where(rank => rank.ProfileId == profileId)
                .Include(rank => rank.Rank)
                .ToListAsync()).Select(rank => rank.Rank)
                .OrderByDescending(rank => rank.MaxExperience)
                .Where(rank => ra)*/
        }
    }
}