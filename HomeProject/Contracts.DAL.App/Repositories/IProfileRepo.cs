using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IProfileRepo : IBaseRepo<Profile>
    {
        Task<Profile> GetProfile(Guid id, Guid? requesterId);
        
        Task<Profile> FindRankIncludeAsync(Guid id);
        Task<Profile> FindByUsernameAsync(string username);
        Task<Profile> FindByUsernameAsync(string username, Guid? requesterId);

        Task IncreaseExperience(Guid userId, int amount);
        Task<Profile> FindByUsernameWithFollowersAsync(string username);
    }
}