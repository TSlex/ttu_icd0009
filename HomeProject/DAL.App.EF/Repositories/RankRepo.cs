using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Domain.Translation;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class RankRepo : BaseRepo<Domain.Rank, Rank, ApplicationDbContext>, IRankRepo
    {
        public RankRepo(ApplicationDbContext dbContext) :
            base(dbContext, new RankMapper())
        {
        }

        public override async Task<Rank> FindAdminAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet
                .Include(rank => rank.RankTitle)
                .ThenInclude(s => s!.Translations)
                .Include(rank => rank.RankDescription)
                .ThenInclude(s => s!.Translations).FirstOrDefaultAsync(rank => rank.Id == id));
        }

        public override async Task<IEnumerable<Rank>> AllAdminAsync()
        {
            return (await RepoDbSet
                    .Include(rank => rank.RankTitle)
                    .ThenInclude(s => s!.Translations)
                    .Include(rank => rank.RankDescription)
                    .ThenInclude(s => s!.Translations)
                    .ToListAsync())
                .Select(rank => Mapper.Map(rank));
        }

        public override async Task<IEnumerable<Rank>> AllAsync()
        {
            return (await RepoDbSet
                    .Include(rank => rank.RankTitle)
                    .ThenInclude(s => s!.Translations)
                    .Include(rank => rank.RankDescription)
                    .ThenInclude(s => s!.Translations)
                    .Where(rank => rank.DeletedAt == null && rank.MasterId == null)
                    .ToListAsync())
                .Select(rank => Mapper.Map(rank));
        }

        public override async Task<Rank> UpdateAsync(Rank entity)
        {
            var domainEntity = await RepoDbSet
                .Include(rank => rank.RankTitle)
                .ThenInclude(s => s!.Translations)
                .Include(rank => rank.RankDescription)
                .ThenInclude(s => s!.Translations)
                .FirstOrDefaultAsync(rank => rank.Id == entity.Id);

            var rankTitle = entity.RankTitle;
            var rankDescription = entity.RankDescription;

            ((LangString) entity.RankTitle).Translations = domainEntity.RankTitle.Translations;
            ((LangString) entity.RankDescription).Translations = domainEntity.RankDescription.Translations;

            ((LangString) entity.RankTitle).Translate(rankTitle);
            ((LangString) entity.RankDescription).Translate(rankDescription);

            return await base.UpdateAsync(entity);
        }

        public async Task<Rank> FindByCodeAsync(string code)
        {
            return Mapper.Map(await RepoDbContext.Ranks
                .Where(rank => rank.DeletedAt == null && rank.MasterId == null)
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
            return (await RepoDbSet.Where(record => record.Id == id || record.MasterId == id).ToListAsync()).Select(
                record => Mapper.Map(record));
        }
    }
}