using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ee.itcollege.aleksi.Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IFollowerRepo : IBaseRepo<Follower>
    {
        Task<Follower> FindAsync(Guid userId, Guid profileId);
        Task<int> CountByIdAsync(Guid userId, bool reversed);
        Task<IEnumerable<Follower>> AllByIdPageAsync(Guid userId, bool reversed, int pageNumber, int count);
    }
}