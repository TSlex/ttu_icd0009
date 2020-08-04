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
            base(uow.Posts, new UniversalBLLMapper<DAL.App.DTO.Post, Post>())
        {
        }

#pragma warning disable 8603
        public async Task<Post> GetPostFull(Guid id)
        {
            //strange asp error. he tries execute function twice with empty id
            if (id == Guid.Empty) return null;
            
            var post = Mapper.Map(await ServiceRepository.FindAsync(id));
            
            post.Comments = post.Comments
                .Where(comment => comment.DeletedAt == null && comment.MasterId == null)
                .ToList();
            
            post.PostCommentsCount = post.Comments!.Count;
            post.PostFavoritesCount = post.Favorites!.Count;

            return post;
        }
#pragma warning restore 8603

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