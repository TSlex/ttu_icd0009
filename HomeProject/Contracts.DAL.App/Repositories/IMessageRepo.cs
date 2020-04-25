using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IMessageRepo : IBaseRepo<Message>
    {
        Task<IEnumerable<Message>> AllAsync(Guid id);
    }
}