using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ChatRoleRepo : BaseRepo<Domain.ChatRole, ChatRole, ApplicationDbContext>, IChatRoleRepo
    {
        public ChatRoleRepo(ApplicationDbContext dbContext) :
            base(dbContext, new ChatRoleMapper())
        {
        }

        public async Task<ChatRole> FindAsync(string chatRoleTitle)
        {
            return Mapper.Map(await RepoDbContext.ChatRoles
                    .FirstOrDefaultAsync((role => role.RoleTitle == chatRoleTitle)));
        }
    }
}