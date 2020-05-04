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

        public async Task<IEnumerable<ProfileGift>> GetByPageAsync(Guid userId, int pageNumber, int onPageCount)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * onPageCount;

            if (pageIndex < 0)
            {
                return new ProfileGift[]{};
            }

            return (await RepoDbContext.ProfileGifts
                .Where(gift => gift.ProfileId == userId)
                .Include(gift => gift.Gift)
                .Skip(startIndex).Take(onPageCount).ToListAsync()).Select(gift => Mapper.Map(gift));

            /*var query = RepoDbContext.ProfileGifts
                .Where(gift => gift.ProfileId == userId)
                .Include(gift => gift.Gift)
                .AsQueryable();

            if (await query.CountAsync() - 1 < startIndex)
            {
                return new ProfileGift[]{};
            }

            var available = await query.Skip(startIndex).CountAsync();

            if (available >= onPageCount)
            {
                return (await query.ToListAsync())
                    .Skip(startIndex)
                    .Take(onPageCount)
                    .Select(gift => Mapper.Map(gift));
            } else
            {
                return (await query.ToListAsync())
                    .Skip(startIndex)
                    .Take(available)
                    .Select(gift => Mapper.Map(gift));
            }*/
        }

        public async Task<int> GetUserCountAsync(Guid userId)
        {
            return await RepoDbContext.ProfileGifts.CountAsync(gift => gift.ProfileId == userId);
        }
    }
}