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
using Rank = Domain.Rank;

namespace DAL.Repositories
{
    public class ChatRoleRepo : BaseRepo<Domain.ChatRole, ChatRole, ApplicationDbContext>, IChatRoleRepo
    {
        public ChatRoleRepo(ApplicationDbContext dbContext) :
            base(dbContext, new ChatRoleMapper())
        {
        }

        public override async Task<ChatRole> FindAdminAsync(Guid id)
        {
            return Mapper.Map(await RepoDbContext.ChatRoles
                .Include(role => role.RoleTitleValue)
                .ThenInclude(s => s.Translations)
                .FirstOrDefaultAsync((role => role.Id == id)));
        }

        public override async Task<IEnumerable<ChatRole>> AllAsync()
        {
            return (await RepoDbSet
                .Where(role => !role.CanEditMembers)
                .Include(role => role.RoleTitleValue)
                .ThenInclude(s => s.Translations)
                .ToListAsync()).Select(role => Mapper.Map(role));
        }

        public override async Task<IEnumerable<ChatRole>> AllAdminAsync()
        {
            return (await RepoDbSet
                .Where(role => role.MasterId == null)
                .Include(role => role.RoleTitleValue)
                .ThenInclude(s => s.Translations)
                .ToListAsync()).Select(role => Mapper.Map(role));
                 }

        public async Task<ChatRole> FindAsync(string chatRoleTitle)
        {
            return Mapper.Map(await RepoDbContext.ChatRoles
                .Include(role => role.RoleTitleValue)
                .ThenInclude(s => s.Translations)
                .FirstOrDefaultAsync((role => role.RoleTitle == chatRoleTitle)));
        }

        public override async Task<ChatRole> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbContext.ChatRoles
                .Include(role => role.RoleTitleValue)
                .ThenInclude(s => s.Translations)
                .FirstOrDefaultAsync((role => role.Id == id)));
        }

        public override async Task<ChatRole> UpdateAsync(ChatRole entity)
        {
            var domainEntity = await RepoDbSet.Include(role => role.RoleTitleValue)
                .ThenInclude(s => s.Translations)
                .FirstOrDefaultAsync((role => role.Id == entity.Id));
            
            domainEntity!.RoleTitleValue.SetTranslation(entity.RoleTitleValue);

            domainEntity.RoleTitle = entity.RoleTitle;

            domainEntity.CanRenameRoom = entity.CanRenameRoom;
            domainEntity.CanEditMembers = entity.CanEditMembers;
            domainEntity.CanEditMessages = entity.CanEditMessages;
            domainEntity.CanEditAllMessages = entity.CanEditAllMessages;
            domainEntity.CanWriteMessages = entity.CanWriteMessages;

            return Mapper.Map(RepoDbSet.Update(domainEntity).Entity);
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
            return (await RepoDbSet.Where(record => record.Id == id || record.MasterId == id).ToListAsync()).Select(
                record => Mapper.Map(record));
        }
    }
}