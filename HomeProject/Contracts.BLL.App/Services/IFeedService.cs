using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IFeedService : IBaseService
    {
        Task<Feed> GetUserFeedAsync(Guid userId);
        Task<Feed> GetCommonFeedAsync();
    }
}