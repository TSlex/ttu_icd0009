using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ee.itcollege.aleksi.Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IBlockedProfileRepo : IBaseRepo<BlockedProfile>
    {
        Task<BlockedProfile> FindAsync(Guid userId, Guid profileId);
        
        Task<int> CountByIdAsync(Guid userId);
        Task<IEnumerable<BlockedProfile>> AllByIdPageAsync(Guid userId, int pageNumber, int count);
    }
}