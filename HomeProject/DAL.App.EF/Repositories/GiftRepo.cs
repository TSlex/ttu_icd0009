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
            return Mapper.Map(await RepoDbSet
                .Include(gift => gift.GiftImage)
                .Include(gift => gift.GiftName)
                .ThenInclude(s => s!.Translations)
                .FirstOrDefaultAsync((gift => gift.Id == id)));
        }

        public override async Task<IEnumerable<Gift>> AllAdminAsync()
        {
            return (await RepoDbSet
                    .Where(gift => gift.MasterId == null)
                    .Include(gift => gift.GiftName)
                    .ThenInclude(s => s!.Translations)
                    .ToListAsync())
                .Select(rank => Mapper.Map(rank));
        }

        public override async Task<IEnumerable<Gift>> AllAsync()
        {
            return (await RepoDbSet
                    .Include(gift => gift.GiftName)
                    .ThenInclude(s => s!.Translations)
                    .Where(gift => gift.DeletedAt == null && gift.MasterId == null)
                    .ToListAsync())
                .Select(rank => Mapper.Map(rank));
        }

        public override async Task<Gift> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbContext.Gifts
                .Include(gift => gift.GiftImage)
                .Include(gift => gift.GiftName)
                .ThenInclude(s => s!.Translations)
                .FirstOrDefaultAsync((gift => gift.Id == id)));
        }

        public async Task<Gift> FindByCodeAsync(string giftCode)
        {
            return Mapper.Map(await RepoDbContext.Gifts
                .Include(gift => gift.GiftName)
                .ThenInclude(s => s!.Translations)
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

            return (await RepoDbContext.Gifts
                    .Include(gift => gift.GiftName)
                    .ThenInclude(s => s!.Translations)
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

        public override Gift Remove(Gift entity)
        {
            var profileGifts = RepoDbContext.ProfileGifts
                .Where(gift => gift.GiftId == entity.Id);

            foreach (var profileGift in profileGifts)
            {
                RepoDbContext.ProfileGifts.Remove(profileGift);
            }

            var imageRecord = RepoDbContext.Images
                .FirstOrDefault(image => image.Id == entity.GiftImageId);

            if (imageRecord != null)
            {
                RepoDbContext.Images.Remove(imageRecord);
            }

            return base.Remove(entity);
        }

        public override async Task<IEnumerable<Gift>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.Where(record => record.Id == id || record.MasterId == id)
                .ToListAsync()).Select(record => Mapper.Map(record));
        }
    }
}