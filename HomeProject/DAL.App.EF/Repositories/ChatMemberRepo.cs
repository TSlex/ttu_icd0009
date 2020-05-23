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

namespace DAL.Repositories
{
    public class ChatMemberRepo : BaseRepo<Domain.ChatMember, ChatMember, ApplicationDbContext>, IChatMemberRepo
    {
        public ChatMemberRepo(ApplicationDbContext dbContext) :
            base(dbContext, new ChatMemberMapper())
        {
        }

        public override async Task<ChatMember> FindAsync(Guid id)
        {
            return Mapper.Map(await RepoDbSet
                .Where(member => member.Id == id)
                .Include(member => member.ChatRole)
                .FirstOrDefaultAsync());
        }

        public async Task<ChatMember> FindByUserAndRoomAsync(Guid userId, Guid chatRoomId)
        {
            return Mapper.Map(await RepoDbSet
                .Include(member => member.ChatRole)
                .Where(member => member.ProfileId == userId && member.ChatRoomId == chatRoomId)
                .FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<ChatMember>> RoomAllAsync(Guid chatRoomId)
        {
            return (await RepoDbContext.ChatMembers
                    .Where(member => member.ChatRoomId == chatRoomId 
                                     && member.DeletedAt == null
                                     && member.MasterId == null)
                    .Include(member => member.ChatRole)
                    .Include(member => member.Profile)
                    .ToListAsync())
                .Select(member => Mapper.Map(member));
        }

        public override async Task<IEnumerable<ChatMember>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.Where(record => record.Id == id || record.MasterId == id).ToListAsync()).Select(record => Mapper.Map(record));
        }
    }
}