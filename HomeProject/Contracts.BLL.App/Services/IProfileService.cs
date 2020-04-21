using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IProfileService: IBaseEntityService<global::DAL.App.DTO.Profile, Profile>
    {
        Task<Profile> GetProfileFull(Guid id);
    }
}