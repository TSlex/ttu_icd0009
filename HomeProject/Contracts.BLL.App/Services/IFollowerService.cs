using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IFollowerService: IBaseEntityService<global::DAL.App.DTO.Follower, Follower>
    {
        Follower AddSubscription(Guid userId, Guid profileId);
        Task<Follower> RemoveSubscriptionAsync(Guid userId, Guid profileId);
        Task<Follower> FindAsync(Guid userId, Guid profileId);
        
        Task<int> CountByIdAsync(Guid userId, bool reversed);
        Task<IEnumerable<Follower>> AllByIdPageAsync(Guid userId, bool reversed, int pageNumber, int count);
    }
}