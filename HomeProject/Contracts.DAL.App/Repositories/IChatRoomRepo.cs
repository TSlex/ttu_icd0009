using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IChatRoomRepo : IBaseRepo<ChatRoom>
    {
        Task<ChatRoom> GetRoomWithUserAsync(Guid firstId, Guid secondId);
        Task<IEnumerable<ChatRoom>> AllAsync(Guid userId);
    }
}