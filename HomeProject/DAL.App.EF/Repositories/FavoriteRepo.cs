using System;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class FavoriteRepo : BaseRepo<Domain.Favorite, Favorite, ApplicationDbContext>, IFavoriteRepo
    {
        public FavoriteRepo(ApplicationDbContext dbContext) :
            base(dbContext, new FavoriteMapper())
        {
        }

        public async Task<Favorite> FindAsync(Guid id, Guid userId)
        {
            return Mapper.Map(
                await RepoDbSet.AsNoTracking()
                    .FirstOrDefaultAsync(favorite =>
                        favorite.PostId == id
                        && favorite.ProfileId == userId));
        }
    }
}