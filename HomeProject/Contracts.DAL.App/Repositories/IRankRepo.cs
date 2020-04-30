using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IRankRepo : IBaseRepo<Rank>
    {
        Task<Rank> FindByCodeAsync(string code);
    }
}