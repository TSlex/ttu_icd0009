using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IProfileRepo : IBaseRepo<Profile>
    {
        Task<Profile> FindAsync(Guid id);
    }
}