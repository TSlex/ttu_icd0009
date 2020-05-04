using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IGiftRepo : IBaseRepo<Gift>
    {
        Task<Gift> FindByCodeAsync(string giftCode);
        Task<IEnumerable<Gift>> Get10ByPageAsync(int pageNumber, int i);
        Task<int> GetCountAsync();
    }
}