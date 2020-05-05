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
    public class ChatRoleService : BaseEntityService<IChatRoleRepo, DAL.App.DTO.ChatRole, ChatRole>, IChatRoleService
    {
        public ChatRoleService(IAppUnitOfWork uow) :
            base(uow.ChatRoles, new ChatRoleMapper())
        {
        }

        public async Task<ChatRole> FindAsync(string chatRoleTitle)
        {
            return Mapper.Map(await ServiceRepository.FindAsync(chatRoleTitle));
        }
    }
}