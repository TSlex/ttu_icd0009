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
    public class ProfileGiftRepo : BaseRepo<Domain.ProfileGift, ProfileGift, ApplicationDbContext>, IProfileGiftRepo
    {
        public ProfileGiftRepo(ApplicationDbContext dbContext) :
            base(dbContext, new ProfileGiftMapper())
        {
        }

        public override async Task<ProfileGift> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet
                .Include(gift => gift.Gift)
                .ThenInclude(gift => gift.GiftName)
                .ThenInclude(s => s.Translations)
                .Include(gift => gift.Profile)
                .Include(gift => gift.FromProfile)
                .FirstOrDefaultAsync(gift => gift.Id == id
                                             && gift.DeletedAt == null));
        }

        public async Task<IEnumerable<ProfileGift>> GetByPageAsync(Guid userId, int pageNumber, int onPageCount)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * onPageCount;

            if (pageIndex < 0)
            {
                return new ProfileGift[] { };
            }

            return (await RepoDbContext.ProfileGifts
                .Where(gift => gift.ProfileId == userId)
                .Include(gift => gift.Gift)
                .Skip(startIndex).Take(onPageCount).ToListAsync()).Select(gift => Mapper.Map(gift));
        }

        public async Task<int> GetUserCountAsync(Guid userId)
        {
            return await RepoDbContext.ProfileGifts.CountAsync(gift => gift.ProfileId == userId);
        }
    }
}