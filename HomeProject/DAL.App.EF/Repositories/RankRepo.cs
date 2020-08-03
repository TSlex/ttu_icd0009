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
        
        public async Task<bool> NextRankExists(Guid id, Guid? reqGuid)
        {
            if (reqGuid == null)
            {
                return await RepoDbSet.Where(rank => 
                               rank.NextRankId == id)
                           .FirstOrDefaultAsync() != null;
            }
            
            return await RepoDbSet.Where(rank => 
                           rank.NextRankId == id && rank.Id != reqGuid)
                       .FirstOrDefaultAsync() != null;
        }
        
        public async Task<bool> PreviousRankExists(Guid id, Guid? reqGuid)
        {
            if (reqGuid == null)
            {
                return await RepoDbSet.Where(rank => 
                               rank.PreviousRankId == id)
                           .FirstOrDefaultAsync() != null;
            }
            
            return await RepoDbSet.Where(rank => 
                           rank.PreviousRankId == id && rank.Id != reqGuid)
                       .FirstOrDefaultAsync() != null;
        }

        public override async Task<Rank> FindAdminAsync(Guid id)
        {
            return Mapper.Map(await GetQuery()
                .FirstOrDefaultAsync(rank => rank.Id == id));
        }

        public override async Task<IEnumerable<Rank>> AllAdminAsync()
        {
            return (await GetQuery()
                    .Where(rank => rank.MasterId == null)
                    .ToListAsync())
                .Select(rank => Mapper.Map(rank));
        }

        public override async Task<IEnumerable<Rank>> AllAsync()
        {
            return (await GetQuery()
                    .Where(rank => rank.DeletedAt == null && rank.MasterId == null)
                    .ToListAsync())
                .Select(rank => Mapper.Map(rank));
        }

        public override async Task<Rank> UpdateAsync(Rank entity)
        {
            var domainEntity = await GetQuery()
                .FirstOrDefaultAsync(rank => rank.Id == entity.Id);

            domainEntity.RankTitle.SetTranslation(entity.RankTitle);
            domainEntity.RankDescription.SetTranslation(entity.RankDescription);

            domainEntity.MaxExperience = entity.MaxExperience;
            domainEntity.MinExperience = entity.MinExperience;
            domainEntity.RankCode = entity.RankCode;
            domainEntity.RankColor = entity.RankColor;
            domainEntity.RankTextColor = entity.RankTextColor;
            domainEntity.RankIcon = entity.RankIcon;
            domainEntity.NextRankId = entity.NextRankId;
            domainEntity.PreviousRankId = entity.PreviousRankId;

            return await base.UpdateDomainAsync(domainEntity);
        }

        public async Task<Rank> FindByCodeAsync(string code)
        {
            return Mapper.Map(await GetQuery()
                .Where(rank =>
                    rank.DeletedAt == null
                    && rank.MasterId == null)
                .FirstOrDefaultAsync(rank => rank.RankCode == code && rank.MasterId == null));
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
            return (await GetQuery()
                    .Where(record => record.Id == id || record.MasterId == id)
                    .ToListAsync())
                .Select(record => Mapper.Map(record));
        }

        private IQueryable<Domain.Rank> GetQuery()
        {
            return RepoDbSet
                .Include(rank => rank.RankTitle)
                .ThenInclude(s => s!.Translations)
                .Include(rank => rank.RankDescription)
                .ThenInclude(s => s!.Translations)
                .AsQueryable();
        }
    }
}