using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IBlockedProfileService: IBaseEntityService<global::DAL.App.DTO.BlockedProfile, BlockedProfile>
    {
        BlockedProfile AddBlockProperty(Guid userId, Guid profileId);
        Task<BlockedProfile?> RemoveBlockPropertyAsync(Guid userId, Guid profileId);
        Task<BlockedProfile> FindAsync(Guid userId, Guid profileId);
        
        Task<int> CountByIdAsync(Guid userId);
        Task<IEnumerable<BlockedProfile>> AllByIdPageAsync(Guid userId, int pageNumber, int count);
    }
}   