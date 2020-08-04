using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Post = Domain.Post;

namespace BLL.App.Services
{
    public class FavoriteService : BaseEntityService<IFavoriteRepo, DAL.App.DTO.Favorite, Favorite>, IFavoriteService
    {
        private readonly IAppUnitOfWork _uow;

        public FavoriteService(IAppUnitOfWork uow) :
            base(uow.Favorites, new UniversalBLLMapper<DAL.App.DTO.Favorite, Favorite>())
        {
            _uow = uow;
        }

        public async Task<Favorite> FindAsync(Guid id, Guid userId)
        {
            return Mapper.Map(await ServiceRepository.FindAsync(id, userId));
        }

        public Favorite? Create(Guid id, Guid userId)
        {
            var post = _uow.Posts.Find(id);

            if (post == null)
            {
                return null;
            }

            return Mapper.Map(ServiceRepository.Add(new DAL.App.DTO.Favorite()
            {
                PostId = id,
                ProfileId = userId,
                PostTitle = post.PostTitle,
                PostDescription = post.PostDescription,
                PostImageId = post.PostImageId
            }));
        }

        public async Task<Favorite> RemoveAsync(Guid id, Guid userId)
        {
            return Mapper.Map(ServiceRepository.Remove(Mapper.MapReverse(await FindAsync(id, userId))));
        }

        public async Task<int> CountByIdAsync(Guid postId)
        {
            return await ServiceRepository.CountByIdAsync(postId);
        }

        public async Task<IEnumerable<Favorite>> AllByPostIdPageAsync(Guid postId, int pageNumber, int count)
        {
            return (await ServiceRepository.AllByIdPageAsync(postId, pageNumber, count)).Select(favorite =>
                Mapper.Map(favorite));
        }
    }
}