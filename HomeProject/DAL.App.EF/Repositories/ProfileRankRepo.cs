using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.aleksi.DAL.Base.EF.Mappers;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;
using Rank = Domain.Rank;

namespace DAL.Repositories
{
    public class ProfileRankRepo : BaseRepo<Domain.ProfileRank, ProfileRank, ApplicationDbContext>, IProfileRankRepo
    {
        public ProfileRankRepo(ApplicationDbContext dbContext) :
            base(dbContext, new UniversalDALMapper<Domain.ProfileRank, ProfileRank>())
        {
        }

        public override async Task<ProfileRank> FindAsync(Guid id)
        {
            return Mapper.Map(await GetQuery()
                .Include(rank => rank.Profile)
                .FirstOrDefaultAsync(rank => rank.Id == id));
        }

        public async Task<IEnumerable<ProfileRank>> AllUserAsync(Guid profileId)
        {
            return await GetQuery()
                .Where(rank =>
                    rank.ProfileId == profileId &&
                    rank.DeletedAt == null)
                .Select(rank => Mapper.Map(rank)).ToListAsync();
        }

        public override ProfileRank Add(ProfileRank entity)
        {
            entity.CreatedBy = "system";
            return base.Add(entity);
        }

        private IQueryable<Domain.ProfileRank> GetQuery()
        {
            return RepoDbSet
                .Include(rank => rank.Rank)
                .ThenInclude(rank => rank!.RankTitle)
                .ThenInclude(title => title!.Translations)
                .Include(rank => rank.Rank)
                .ThenInclude(rank => rank!.RankDescription)
                .ThenInclude(desc => desc!.Translations)
                .AsQueryable();
        }
    }
}