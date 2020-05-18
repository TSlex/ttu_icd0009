using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;
using ChatRoom = Domain.ChatRoom;

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

        public override Task<ChatRole> UpdateAsync(ChatRole entity)
        {
            throw new NotSupportedException();
        }

        public override ChatRole Remove(ChatRole entity)
        {
            var members = RepoDbContext.ChatMembers.Where(member => member.ChatRoleId == entity.Id).ToList();

            foreach (var chatMember in members)
            {
                RepoDbContext.ChatMembers.Remove(chatMember);
            }
            
            return base.Remove(entity);
        }
        
        public override async Task<IEnumerable<ChatRole>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.Where(record => record.Id == id || record.MasterId == id).ToListAsync()).Select(record => Mapper.Map(record));
        }
    }
}