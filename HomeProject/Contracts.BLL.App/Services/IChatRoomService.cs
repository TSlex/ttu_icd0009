using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IChatRoomService: IBaseEntityService<global::DAL.App.DTO.ChatRoom, ChatRoom>
    {
        Task<Guid?> OpenOrCreateAsync(string username);
    }
}