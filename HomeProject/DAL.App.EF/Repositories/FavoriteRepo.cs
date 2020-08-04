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
    public class FavoriteRepo : BaseRepo<Domain.Favorite, Favorite, ApplicationDbContext>, IFavoriteRepo
    {
        public FavoriteRepo(ApplicationDbContext dbContext) :
            base(dbContext, new UniversalDALMapper<Domain.Favorite, Favorite>())
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

        public async Task<IEnumerable<Favorite>> AllByIdPageAsync(Guid postId, int pageNumber, int count)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * count;

            if (pageIndex < 0)
            {
                return new Favorite[] { };
            }

            return (await RepoDbContext.Favorites
                    .Where(favorite => favorite.PostId == postId)
                    .Include(favorite => favorite.Profile)
                    .OrderByDescending(favorite => favorite.Post!.PostPublicationDateTime)
                    .Skip(startIndex)
                    .Take(count)
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }

        public async Task<int> CountByIdAsync(Guid postId)
        {
            return await RepoDbSet.CountAsync(favorite => favorite.PostId == postId);
        }
    }
}