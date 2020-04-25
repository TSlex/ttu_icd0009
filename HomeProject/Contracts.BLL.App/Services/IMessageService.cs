using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IMessageService: IBaseEntityService<global::DAL.App.DTO.Message, Message>
    {
        Task<IEnumerable<Message>> AllAsync(Guid id);
    }
}