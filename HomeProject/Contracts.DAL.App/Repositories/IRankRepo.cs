using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IRankRepo : IBaseRepo<Rank>
    {
        Task<bool> NextRankExists(Guid id, Guid? reqGuid);
        Task<bool> PreviousRankExists(Guid id, Guid? reqGuid);
        
        Task<Rank> FindByCodeAsync(string code);
    }
}