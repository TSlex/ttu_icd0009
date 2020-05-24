using System;
using System.Collections.Generic;
using System.Linq;
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
            
            post.Comments = post.Comments
                .Where(comment => comment.DeletedAt == null && comment.MasterId == null)
                .ToList();
            
            post.PostCommentsCount = post.Comments.Count;
            post.PostFavoritesCount = post.Favorites.Count;

            return post;
        }

        public async Task<Post> GetNoIncludes(Guid id, Guid? requesterId)
        {
            return Mapper.Map(await ServiceRepository.GetNoIncludes(id, requesterId));
        }

        public async Task<int> GetFavoritesCount(Guid id)
        {
            return await ServiceRepository.GetFavoritesCount(id);
        }

        public async Task<int> GetCommentsCount(Guid id)
        {
            return await ServiceRepository.GetCommentsCount(id);
        }

        public async Task<int> GetByUserCount(Guid userId)
        {
            return await ServiceRepository.GetByUserCount(userId);
        }

        public async Task<IEnumerable<Post>> GetUser10ByPage(Guid userId, int pageNumber, Guid? requesterId)
        {
            return (await ServiceRepository.GetUserByPage(userId, pageNumber, 10, requesterId)).Select(post => Mapper.Map(post));
        }
        
        public async Task<bool> IsUserFavorite(Guid postId, Guid userId)
        {
            return (await ServiceRepository.IsUserFavorite(postId, userId));
        }
    }
}