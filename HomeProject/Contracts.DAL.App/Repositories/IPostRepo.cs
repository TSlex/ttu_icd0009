using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IPostRepo : IBaseRepo<Post>
    {
        Task<IEnumerable<Post>> GetUserFollowsPostsAsync(Guid userId);
        Task<IEnumerable<Post>> GetCommonFeedAsync();

        Task<int> GetByUserCount(Guid userId);
        Task<IEnumerable<Post>> GetUserByPage(Guid userId, int pageNumber, int onPageCount, Guid? requesterId);
        
        Task<int> GetCommentsCount(Guid id);
        Task<int> GetFavoritesCount(Guid id);
        Task<Post> GetNoIncludes(Guid id, Guid? requesterId);
        
        Task<int> GetUserFollowsPostsCount(Guid userId);
        Task<int> GetCommonPostsCount();
        
        Task<IEnumerable<Post>> GetUserFollowsPostsByPage(Guid userId, int pageNumber, int i);
        Task<IEnumerable<Post>> GetCommonFeedByPage(int pageNumber, int i);

        Task<bool> IsUserFavorite(Guid postId, Guid userId);
    }
}