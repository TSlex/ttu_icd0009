using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IRankService: IBaseEntityService<global::DAL.App.DTO.Rank, Rank>
    {
        Task<Rank> FindByCodeAsync(string s);
        Task IncreaseUserExperience(Guid userId, int amount);
    }
}