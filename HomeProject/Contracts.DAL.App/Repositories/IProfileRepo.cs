using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IProfileRepo : IBaseRepo<Profile>
    {
        Task<Profile> FindFullIncludeAsync(Guid id);
        Task<Profile> FindNoIncludeAsync(Guid id);
        Task<Profile> FindByUsernameAsync(string username);
        
        Task IncreaseExperience(Guid userId, int amount);
    }
}