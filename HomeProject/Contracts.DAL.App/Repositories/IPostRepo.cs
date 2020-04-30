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
    }
}