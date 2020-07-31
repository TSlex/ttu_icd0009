using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IProfileService : IBaseEntityService<global::DAL.App.DTO.Profile, Profile>
    {
        Task<bool> ExistsAsync(string username);
        
        Task<Profile> GetProfileAsync(Guid id, Guid? requesterId);

        Task<Profile> FindByUsernameAsync(string username);
        Task<Profile> FindByUsernameAsync(string username, Guid? requesterId);
        Task<Profile> FindByUsernameWithFollowersAsync(string username);
    }
}