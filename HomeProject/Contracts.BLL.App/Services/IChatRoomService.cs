using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IChatRoomService: IBaseEntityService<global::DAL.App.DTO.ChatRoom, ChatRoom>
    {
        Task<Guid?> OpenOrCreateAsync(string username);
        Task<IEnumerable<ChatRoom>> AllAsync(Guid userId);
        
        Task<bool> IsRoomMemberAsync(Guid chatRoomId, Guid userId);
        Task<bool> IsRoomAdministratorAsync(Guid id, Guid userId);
    }
}