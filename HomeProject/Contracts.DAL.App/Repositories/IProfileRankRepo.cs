using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ee.itcollege.aleksi.Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IProfileRankRepo : IBaseRepo<ProfileRank>
    {
        Task<IEnumerable<ProfileRank>> AllUserAsync(Guid profileId);
    }
}