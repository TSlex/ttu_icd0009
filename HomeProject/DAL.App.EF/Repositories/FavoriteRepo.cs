using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Mappers;

namespace DAL.Repositories
{
    public class FavoriteRepo : BaseRepo<Domain.Favorite, Favorite, ApplicationDbContext>, IFavoriteRepo
    {
        public FavoriteRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new FavoriteMapper())
        {
        }
    }
}