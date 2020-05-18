using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IChatMemberRepo : IBaseRepo<ChatMember>
    {
        Task<ChatMember> FindByUserAndRoomAsync(Guid userId, Guid chatRoomId);
        Task<IEnumerable<ChatMember>> RoomAllAsync(Guid chatRoomId);
    }
}