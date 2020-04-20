using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;

namespace DAL.Repositories
{
    public class FavoriteRepo : BaseRepo<Domain.Favorite, Favorite, ApplicationDbContext>, IFavoriteRepo
    {
        public FavoriteRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new BaseDALMapper<Domain.Favorite, Favorite>())
        {
        }
    }
}