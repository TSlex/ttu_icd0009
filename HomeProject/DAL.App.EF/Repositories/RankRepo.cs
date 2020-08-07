using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.aleksi.DAL.Base.EF.Mappers;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;
using DAL.Mappers;
using Domain.Translation;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class RankRepo : BaseRepo<Domain.Rank, Rank, ApplicationDbContext>, IRankRepo
    {
        public RankRepo(ApplicationDbContext dbContext) :
            base(dbContext, new UniversalDALMapper<Domain.Rank, Rank>())
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

            domainEntity.RankTitle!.SetTranslation(entity.RankTitle!);
            domainEntity.RankDescription!.SetTranslation(entity.RankDescription!);

            domainEntity.MaxExperience = entity.MaxExperience;
            domainEntity.MinExperience = entity.MinExperience;
            domainEntity.RankCode = entity.RankCode;
            domainEntity.RankColor = entity.RankColor;
            domainEntity.RankTextColor = entity.RankTextColor;
            domainEntity.RankIcon = entity.RankIcon;
            domainEntity.NextRankId = entity.NextRankId;
            domainEntity.PreviousRankId = entity.PreviousRankId;

            //==================================================

            var trackEntity = RepoDbSet.Find(entity.Id);
            var newEntity = domainEntity;
            
            var now = DateTime.UtcNow;

            newEntity.CreatedAt = now;
            
            RepoDbContext.Entry(trackEntity).State = EntityState.Detached;

            trackEntity.NextRankId = null;
            trackEntity.PreviousRankId = null;
            
            trackEntity.MasterId = newEntity.Id;
            trackEntity.Id = Guid.NewGuid();
            trackEntity.ChangedBy = "history";
            trackEntity.ChangedAt = now;
            trackEntity.DeletedBy = "history";
            trackEntity.DeletedAt = now;
            
            RepoDbSet.Add(trackEntity);
            
            return Mapper.Map(RepoDbSet.Update(newEntity).Entity);
        }

        public async Task<Rank> FindByCodeAsync(string code)
        {
            return Mapper.Map(await GetQuery()
                .Where(rank =>
                    rank.DeletedAt == null
                    && rank.MasterId == null)
                .FirstOrDefaultAsync(rank => rank.RankCode == code && rank.MasterId == null));
        }

        public override Rank Remove(Rank tEntity)
        {
            var ranks = RepoDbContext.ProfileRanks.Where(rank => rank.RankId == tEntity.Id).ToList();

            foreach (var rank in ranks)
            {
                RepoDbContext.ProfileRanks.Remove(rank);
            }

            return base.Remove(tEntity);
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