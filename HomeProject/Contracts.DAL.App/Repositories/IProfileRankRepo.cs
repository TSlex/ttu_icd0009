using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IProfileRankRepo : IBaseRepo<ProfileRank>
    {
        Task<IEnumerable<ProfileRank>> AllUserAsync(Guid profileId);
    }
}