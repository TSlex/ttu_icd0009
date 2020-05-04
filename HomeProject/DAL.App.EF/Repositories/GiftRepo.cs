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

        public override async Task<Gift> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbContext.Gifts
                .Include(gift => gift.GiftImage)
                .FirstOrDefaultAsync((gift => gift.Id == id)));
        }

        public async Task<Gift> FindByCodeAsync(string giftCode)
        {
            return Mapper.Map(await RepoDbContext.Gifts
                .FirstOrDefaultAsync((gift => gift.GiftCode == giftCode)));
        }

        public async Task<IEnumerable<Gift>> Get10ByPageAsync(int pageNumber, int onPageCount)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * onPageCount;

            if (pageIndex < 0)
            {
                return new Gift[]{};
            }

            return (await RepoDbContext.Gifts
                .Skip(startIndex).Take(onPageCount).ToListAsync()).Select(gift => Mapper.Map(gift));
        }

        public async Task<int> GetCountAsync()
        {
            return await RepoDbContext.Gifts.CountAsync();
        }
    }
}