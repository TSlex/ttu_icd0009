using System;
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
    }
}