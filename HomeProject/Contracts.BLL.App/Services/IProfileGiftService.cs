using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IProfileGiftService: IBaseEntityService<global::DAL.App.DTO.ProfileGift, ProfileGift>
    {
        Task<IEnumerable<ProfileGift>> GetUser10ByPageAsync(Guid userId, int pageNumber);
        Task<GiftsCount> GetUserCountAsync(Guid userId);
    }
}