using System;
using System.Threading.Tasks;
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

        public async Task<Favorite> FindAsync(Guid id, Guid userId)
        {
            return Mapper.Map(await ServiceRepository.FindAsync(id, userId));
        }

        public Favorite Create(Guid id, Guid userId)
        {
            return Mapper.Map(ServiceRepository.Add(new DAL.App.DTO.Favorite()
            {
                PostId = id,
                ProfileId = userId
            }));
        }

        public async Task<Favorite> RemoveAsync(Guid id, Guid userId)
        {
            return Mapper.Map(ServiceRepository.Remove(Mapper.MapReverse(await FindAsync(id, userId))));
        }
    }
}