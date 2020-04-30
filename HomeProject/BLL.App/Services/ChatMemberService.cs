using BLL.App.DTO;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class ChatMemberService : BaseEntityService<IChatMemberRepo, DAL.App.DTO.ChatMember, ChatMember>, IChatMemberService
    {
        public ChatMemberService(IAppUnitOfWork uow) :
            base(uow.ChatMembers, new ChatMemberMapper())
        {
        }
    }
}