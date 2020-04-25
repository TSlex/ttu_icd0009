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
            base(uow.Messages, new MessageMapper())
        {
        }
    }
}