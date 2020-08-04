using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.aleksi.DAL.Base.EF.Mappers;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;
using ChatRoom = Domain.ChatRoom;
using Rank = Domain.Rank;

namespace DAL.Repositories
{
    public class ChatRoleRepo : BaseRepo<Domain.ChatRole, ChatRole, ApplicationDbContext>, IChatRoleRepo
    {
        public ChatRoleRepo(ApplicationDbContext dbContext) :
            base(dbContext, new UniversalDALMapper<Domain.ChatRole, ChatRole>())
        {
        }

        public override async Task<ChatRole> FindAdminAsync(Guid id)
        {
            return Mapper.Map(await GetQuery()
                .FirstOrDefaultAsync((role => role.Id == id)));
        }

        public override async Task<IEnumerable<ChatRole>> AllAsync()
        {
            return (await GetQuery()
                .Where(role => !role.CanEditMembers)
                .ToListAsync()).Select(role => Mapper.Map(role));
        }

        public override async Task<IEnumerable<ChatRole>> AllAdminAsync()
        {
            return (await GetQuery()
                .Where(role => role.MasterId == null)
                .ToListAsync())
                .Select(role => Mapper.Map(role));}

        public async Task<ChatRole> FindAsync(string chatRoleTitle)
        {
            return Mapper.Map(await GetQuery()
                .FirstOrDefaultAsync((role => role.RoleTitle == chatRoleTitle)));
        }

        public override async Task<ChatRole> FindAsync(Guid id)
        {
            return Mapper.Map(await GetQuery()
                .FirstOrDefaultAsync((role => role.Id == id)));
        }

        public override async Task<ChatRole> UpdateAsync(ChatRole entity)
        {
            var domainEntity = await GetQuery()
                .FirstOrDefaultAsync((role => role.Id == entity.Id));
            
            domainEntity!.RoleTitleValue!.SetTranslation(entity.RoleTitleValue!);

            domainEntity.RoleTitle = entity.RoleTitle;

            domainEntity.CanRenameRoom = entity.CanRenameRoom;
            domainEntity.CanEditMembers = entity.CanEditMembers;
            domainEntity.CanEditMessages = entity.CanEditMessages;
            domainEntity.CanEditAllMessages = entity.CanEditAllMessages;
            domainEntity.CanWriteMessages = entity.CanWriteMessages;

            return await base.UpdateDomainAsync(domainEntity);
        }

        public override ChatRole Remove(ChatRole tEntity)
        {
            var members = RepoDbContext.ChatMembers.Where(member => member.ChatRoleId == tEntity.Id).ToList();

            foreach (var chatMember in members)
            {
                RepoDbContext.ChatMembers.Remove(chatMember);
            }

            return base.Remove(tEntity);
        }

        public override async Task<IEnumerable<ChatRole>> GetRecordHistoryAsync(Guid id)
        {
            return (await GetQuery()
                .Where(record => record.Id == id || record.MasterId == id)
                .ToListAsync()).Select(role => Mapper.Map(role));
        }
        
        private IQueryable<Domain.ChatRole> GetQuery()
        {
            return RepoDbSet
                .Include(role => role.RoleTitleValue!)
                .ThenInclude(s => s.Translations!)
                .AsQueryable();
        }
    }
}