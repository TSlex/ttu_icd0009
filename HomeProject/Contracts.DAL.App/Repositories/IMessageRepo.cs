using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ee.itcollege.aleksi.Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IMessageRepo : IBaseRepo<Message>
    {
        Task<IEnumerable<Message>> AllAsync(Guid id);
        Task<IEnumerable<Message>> AllByIdPageAsync(Guid chatRoomId, int pageNumber, int count);
        Task<int> CountByRoomAsync(Guid chatRoomId);
        Task<Message> GetLastMessage(Guid chatRoomId);
    }
}