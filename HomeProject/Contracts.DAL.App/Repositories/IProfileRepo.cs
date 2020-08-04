using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using ee.itcollege.aleksi.Contracts.DAL.Base.Repositories;
using Profile = DAL.App.DTO.Profile;

namespace Contracts.DAL.App.Repositories
{
    public interface IProfileRepo : IBaseRepo<Profile>
    {
        Task<bool> ExistsAsync(string username);

        Task<Tuple<ProfileEdit, string[]>> UpdateProfileAdminAsync(ProfileEdit entity);
        
        Task<Profile> GetProfile(Guid id, Guid? requesterId);
        
        Task<Profile> FindRankIncludeAsync(Guid id);
        Task<Profile> FindByUsernameAsync(string username);
        Task<Profile> FindByUsernameAsync(string username, Guid? requesterId);

        Task IncreaseExperience(Guid userId, int amount);
        Task<Profile> FindByUsernameWithFollowersAsync(string username);
    }
}