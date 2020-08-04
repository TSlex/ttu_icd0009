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
    public class GiftRepo : BaseRepo<Domain.Gift, Gift, ApplicationDbContext>, IGiftRepo
    {
        public GiftRepo(ApplicationDbContext dbContext) :
            base(dbContext, new GiftMapper())
        {
        }

        public override async Task<Gift> FindAdminAsync(Guid id)
        {
            return Mapper.Map(await GetQuery()
                .Include(gift => gift.GiftImage)
                .FirstOrDefaultAsync((gift => gift.Id == id)));
        }

        public override async Task<IEnumerable<Gift>> AllAdminAsync()
        {
            return (await GetQuery()
                    .Where(gift => gift.MasterId == null)
                    .ToListAsync())
                .Select(gift => Mapper.Map(gift));
        }

        public override async Task<IEnumerable<Gift>> AllAsync()
        {
            return (await GetQuery()
                    .Where(gift => gift.DeletedAt == null && gift.MasterId == null)
                    .ToListAsync())
                .Select(rank => Mapper.Map(rank));
        }

        public override async Task<Gift> FindAsync(Guid id)
        {
            return Mapper.Map(await GetQuery()
                .Include(gift => gift.GiftImage)
                .FirstOrDefaultAsync((gift => gift.Id == id)));
        }
        
        public override async Task<Gift> UpdateAsync(Gift entity)
        {
            var domainEntity = await GetQuery()
                .FirstOrDefaultAsync(rank => rank.Id == entity.Id);
            
            domainEntity.GiftName!.SetTranslation(entity.GiftName!);

            domainEntity.Price = entity.Price;
            domainEntity.GiftCode = entity.GiftCode;
            domainEntity.GiftImageId = entity.GiftImageId;

            return await base.UpdateDomainAsync(domainEntity);
        }

        public async Task<Gift> FindByCodeAsync(string giftCode)
        {
            return Mapper.Map(await GetQuery()
                .Where(gift => gift.DeletedAt == null && gift.MasterId == null)
                .FirstOrDefaultAsync(gift => gift.GiftCode == giftCode && gift.MasterId == null));
        }

        public async Task<IEnumerable<Gift>> Get10ByPageAsync(int pageNumber, int onPageCount)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * onPageCount;

            if (pageIndex < 0)
            {
                return new Gift[] { };
            }

            return (await GetQuery()
                    .Where(gift => gift.DeletedAt == null && gift.MasterId == null)
                    .Skip(startIndex).Take(onPageCount)
                    .ToListAsync())
                .Select(gift => Mapper.Map(gift));
        }

        public async Task<int> GetCountAsync()
        {
            return await RepoDbContext.Gifts
                .Where(gift => gift.DeletedAt == null && gift.MasterId == null).CountAsync();
        }

        public override Gift Remove(Gift tEntity)
        {
            var profileGifts = RepoDbContext.ProfileGifts
                .Where(gift => gift.GiftId == tEntity.Id);

            foreach (var profileGift in profileGifts)
            {
                RepoDbContext.ProfileGifts.Remove(profileGift);
            }

            var imageRecord = RepoDbContext.Images
                .FirstOrDefault(image => image.Id == tEntity.GiftImageId);

            if (imageRecord != null)
            {
                RepoDbContext.Images.Remove(imageRecord);
            }

            return base.Remove(tEntity);
        }

        public override async Task<IEnumerable<Gift>> GetRecordHistoryAsync(Guid id)
        {
            return (await GetQuery()
                .Where(record => record.Id == id || record.MasterId == id)
                .ToListAsync()).Select(record => Mapper.Map(record));
        }
        
        private IQueryable<Domain.Gift> GetQuery()
        {
            return RepoDbSet
                .Include(gift => gift.GiftName)
                .ThenInclude(s => s!.Translations)
                .AsQueryable();
        }
    }
}