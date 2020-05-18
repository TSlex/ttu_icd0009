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
    public class RankRepo : BaseRepo<Domain.Rank, Rank, ApplicationDbContext>, IRankRepo
    {
        public RankRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new RankMapper())
        {
        }

        public async Task<Rank> FindByCodeAsync(string code)
        {
            return Mapper.Map(await RepoDbContext.Ranks
                .Where(rank => rank.DeletedAt == null)
                .Include(rank => rank.RankTitle)
                .ThenInclude(s => s!.Translations)
                .Include(rank => rank.RankDescription)
                .ThenInclude(s => s!.Translations)
                .FirstOrDefaultAsync(rank => rank.RankCode == code));
        }

        public override Rank Remove(Rank entity)
        {
            var ranks = RepoDbContext.ProfileRanks.Where(rank => rank.RankId == entity.Id).ToList();

            foreach (var rank in ranks)
            {
                RepoDbContext.ProfileRanks.Remove(rank);
            }
            
            return base.Remove(entity);
        }
        
        public override async Task<IEnumerable<Rank>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.Where(record => record.Id == id || record.MasterId == id).ToListAsync()).Select(record => Mapper.Map(record));
        }
    }
}