using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IProfileRankService: IBaseEntityService<global::DAL.App.DTO.ProfileRank, ProfileRank>
    {
        Task<IEnumerable<ProfileRank>> AllUserAsync(Guid profileId);
    }
}