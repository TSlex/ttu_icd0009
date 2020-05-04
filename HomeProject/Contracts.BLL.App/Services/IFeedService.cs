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
        
        Task<int> GetUserCount(Guid userId);
        Task<int> GetCount();
        Task<Feed> GetUser10ByPage(Guid userId, int pageNumber);
        Task<Feed> Get10ByPage(int pageNumber);
    }
}