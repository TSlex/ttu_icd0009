using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IProfileService: IBaseEntityService<global::DAL.App.DTO.Profile, ProfileFull>
    {
        Task<ProfileFull> GetProfileFull(Guid id);
        Task<ProfileLimited> GetProfileLimited(Guid id);

        Task<ProfileFull> FindByUsernameAsync(string username);
    }
}