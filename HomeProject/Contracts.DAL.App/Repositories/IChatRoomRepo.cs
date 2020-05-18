using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IChatRoomRepo : IBaseRepo<ChatRoom>
    {
        Task<ChatRoom> GetRoomWithUserAsync(Guid userId, Guid requesterId);
        Task<IEnumerable<ChatRoom>> AllAsync(Guid userId);
        
        Task<bool> IsRoomMemberAsync(Guid chatRoomId, Guid userId);
        Task<bool> IsRoomAdministratorAsync(Guid chatRoomId, Guid userId);
    }
}