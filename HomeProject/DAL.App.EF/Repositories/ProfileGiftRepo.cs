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

        public override async Task<IEnumerable<ProfileGift>> AllAdminAsync()
        {
            return await RepoDbSet.Include(gift => gift.Gift)
                .Select(gift => Mapper.Map(gift)).ToListAsync();
        }

        public override async Task<ProfileGift> FindAdminAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet.Include(gift => gift.Gift)
                .FirstOrDefaultAsync(gift => gift.Id == id));
        }

        public override async Task<ProfileGift> FindAsync(Guid id)
        {
            return Mapper.Map(await GetQuery(GetFindBaseQuery(id))
                    .FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<ProfileGift>> GetByPageAsync(Guid userId, int pageNumber, int onPageCount)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * onPageCount;

            if (pageIndex < 0)
            {
                return new ProfileGift[] { };
            }

            return (await GetQuery(GetAllBaseQuery(userId))
                    .OrderByDescending(gift => gift.GiftDateTime)
                    .Skip(startIndex)
                    .Take(onPageCount)
                    .ToListAsync())
                .Select(gift => Mapper.Map(gift));
        }
        
        public async Task<int> GetUserCountAsync(Guid userId)
        {
            return await RepoDbContext.ProfileGifts.CountAsync(gift => gift.ProfileId == userId);
        }

        private IQueryable<Domain.ProfileGift> GetFindBaseQuery(Guid id)
        {
            return RepoDbSet.Where(gift => gift.Id == id);
        }

        private IQueryable<Domain.ProfileGift> GetAllBaseQuery(Guid userId)
        {
            return RepoDbSet.Where(gift => gift.ProfileId == userId);
        }

        private static IQueryable<Domain.ProfileGift> GetQuery(IQueryable<Domain.ProfileGift> baseQuery)
        {
            return baseQuery
                .Where(gift => gift.DeletedAt == null)
                .Include(gift => gift.Gift)
                .ThenInclude(gift => gift!.GiftName)
                .ThenInclude(s => s!.Translations)
                .Include(gift => gift.Profile)
                .Include(gift => gift.FromProfile)
                .AsQueryable();
        }
    }
}