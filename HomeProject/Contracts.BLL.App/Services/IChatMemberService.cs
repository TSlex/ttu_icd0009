using BLL.App.DTO;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IChatMemberService: IBaseEntityService<global::DAL.App.DTO.ChatMember, ChatMember>
    {
        
    }
}