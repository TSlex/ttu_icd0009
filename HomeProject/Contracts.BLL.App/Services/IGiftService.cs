using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IGiftService: IBaseEntityService<global::DAL.App.DTO.Gift, Gift>
    {
        Task<Gift> FindByCodeAsync(string giftCode);
        Task<GiftsCount> GetCountAsync();
        Task<IEnumerable<Gift>> Get10ByPageAsync(int pageNumber);
    }
}