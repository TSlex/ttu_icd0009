using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class MessageService : BaseEntityService<IMessageRepo, DAL.App.DTO.Message, Message>, IMessageService
    {
        public MessageService(IAppUnitOfWork uow) :
            base(uow.Messages, new UniversalBLLMapper<DAL.App.DTO.Message, Message>())
        {
        }

        public async Task<IEnumerable<Message>> AllAsync(Guid id)
        {
            return (await ServiceRepository.AllAsync(id)).Select(message => Mapper.Map(message));
        }

        public async Task<IEnumerable<Message>> AllByIdPageAsync(Guid chatRoomId, int pageNumber, int count)
        {
            return (await ServiceRepository.AllByIdPageAsync(chatRoomId, pageNumber, count)).Select(
                message => Mapper.Map(message));
        }

        public async Task<int> CountByRoomAsync(Guid chatRoomId)
        {
            return await ServiceRepository.CountByRoomAsync(chatRoomId);
        }

        public async Task<Message> GetLastMessage(Guid chatRoomId)
        {
            return Mapper.Map(await ServiceRepository.GetLastMessage(chatRoomId));
        }
    }
}