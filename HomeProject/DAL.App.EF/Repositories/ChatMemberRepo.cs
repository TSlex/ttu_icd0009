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

namespace DAL.Repositories
{
    public class ChatMemberRepo : BaseRepo<Domain.ChatMember, ChatMember, ApplicationDbContext>, IChatMemberRepo
    {
        public ChatMemberRepo(ApplicationDbContext dbContext) :
            base(dbContext, new UniversalDALMapper<Domain.ChatMember, ChatMember>())
        {
        }

        public override async Task<ChatMember> FindAsync(Guid id)
        {
            return Mapper.Map(await GetQuery()
                .Where(member => member.Id == id)
                .FirstOrDefaultAsync());
        }

        public async Task<ChatMember> FindByUserAndRoomAsync(Guid userId, Guid chatRoomId)
        {
            return Mapper.Map(await GetQuery()
                .Where(member => member.ProfileId == userId && member.ChatRoomId == chatRoomId)
                .FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<ChatMember>> RoomAllAsync(Guid chatRoomId)
        {
            return (await GetQuery()
                    .Where(member =>
                        member.ChatRoomId == chatRoomId &&
                        member.DeletedAt == null &&
                        member.MasterId == null)
                    .ToListAsync())
                .Select(member => Mapper.Map(member));
        }

        public override async Task<IEnumerable<ChatMember>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.Where(record => record.Id == id || record.MasterId == id).ToListAsync()).Select(
                record => Mapper.Map(record));
        }

        private IQueryable<Domain.ChatMember> GetQuery()
        {
            return RepoDbSet
                .Include(member => member.ChatRole)
                .ThenInclude(role => role!.RoleTitleValue)
                .ThenInclude(s => s!.Translations)
                .Include(member => member.Profile)
                .AsQueryable();
        }
    }
}