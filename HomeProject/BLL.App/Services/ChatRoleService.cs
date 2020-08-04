using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.Mappers;
using ee.itcollege.aleksi.BLL.Base.Services;
using Contracts.BLL.App.Services;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class ChatRoleService : BaseEntityService<IChatRoleRepo, DAL.App.DTO.ChatRole, ChatRole>, IChatRoleService
    {
        public ChatRoleService(IAppUnitOfWork uow) :
            base(uow.ChatRoles, new UniversalBLLMapper<DAL.App.DTO.ChatRole, ChatRole>())
        {
        }

        public async Task<ChatRole> FindAsync(string chatRoleTitle)
        {
            return Mapper.Map(await ServiceRepository.FindAsync(chatRoleTitle));
        }
    }
}