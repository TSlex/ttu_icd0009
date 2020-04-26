using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class PostService : BaseEntityService<IPostRepo, DAL.App.DTO.Post, Post>, IPostService
    {
        public PostService(IAppUnitOfWork uow) :
            base(uow.Posts, new PostMapper())
        {
        }

        public async Task<Post> GetPostFull(Guid id)
        {
            var post = Mapper.Map(await ServiceRepository.FindAsync(id));
            post.PostCommentsCount = post.Comments?.Count ?? 0;
            post.PostFavoritesCount = post.Favorites?.Count ?? 0;

            return post;
        }
    }
}