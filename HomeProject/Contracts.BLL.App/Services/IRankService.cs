using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IRankService: IBaseEntityService<global::DAL.App.DTO.Rank, Rank>
    {
        Task<bool> NextRankExists(Guid id, Guid? reqGuid);
        Task<bool> PreviousRankExists(Guid id, Guid? reqGuid);
        
        Task<Rank> FindByCodeAsync(string s);
        Task IncreaseUserExperience(Guid userId, int amount);
    }
}