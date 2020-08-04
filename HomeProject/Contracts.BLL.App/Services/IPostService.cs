using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IPostService: IBaseEntityService<global::DAL.App.DTO.Post, Post>
    {
        Task<Post> GetPostFull(Guid id);
        Task<Post> GetNoIncludes(Guid id, Guid? requesterId);
        
        Task<int> GetFavoritesCount(Guid id);
        Task<int> GetCommentsCount(Guid id);
        Task<int> GetByUserCount(Guid userId);
        Task<IEnumerable<Post>> GetUser10ByPage(Guid userId, int pageNumber, Guid? requesterId);
        
        Task<bool> IsUserFavorite(Guid postId, Guid userId);
    }
}