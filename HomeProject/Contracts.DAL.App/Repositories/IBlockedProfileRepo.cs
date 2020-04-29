using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IBlockedProfileRepo : IBaseRepo<BlockedProfile>
    {
        Task<BlockedProfile> FindAsync(Guid userId, Guid profileId);
    }
}