using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class FavoriteService : BaseEntityService<IFavoriteRepo, DAL.App.DTO.Favorite, Favorite>, IFavoriteService
    {
        public FavoriteService(IAppUnitOfWork uow) :
            base(uow.Favorites, new FavoriteMapper())
        {
        }
    }
}