using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IProfileGiftRepo : IBaseRepo<ProfileGift>
    {
        Task<int> GetUserCountAsync(Guid userId);
        Task<IEnumerable<ProfileGift>> GetByPageAsync(Guid userId, int pageNumber, int onPageCount);
    }
}