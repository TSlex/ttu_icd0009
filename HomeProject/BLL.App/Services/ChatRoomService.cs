using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class ChatRoomService : BaseEntityService<IChatRoomRepo, DAL.App.DTO.ChatRoom, ChatRoom>, IChatRoomService
    {
        public ChatRoomService(IAppUnitOfWork uow) :
            base(uow.ChatRooms, new ChatRoomMapper())
        {
        }
    }
}