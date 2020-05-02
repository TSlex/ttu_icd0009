using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ChatRoomRepo : BaseRepo<Domain.ChatRoom, ChatRoom, ApplicationDbContext>, IChatRoomRepo
    {
        public ChatRoomRepo(ApplicationDbContext dbContext) :
            base(dbContext, new ChatRoomMapper())
        {
        }

        public async Task<ChatRoom> GetRoomWithUserAsync(Guid firstId, Guid secondId)
        {
//            return Mapper.Map(await RepoDbContext.ChatRooms
//                .Include(room => room.ChatMembers)
//                .Where(room => room.ChatMembers.Count == 2)
//                .Where(room => room.ChatMembers
//                    .Select(member => member.ProfileId)
//                    .Contains(firstId))
//                .Where(room => room.ChatMembers
//                    .Select(member => member.ProfileId)
//                    .Contains(secondId))
//                .FirstOrDefaultAsync());

            return Mapper.Map(await RepoDbContext.ChatRooms
                .Include(room => room.ChatMembers)
                .Where(room => room.ChatMembers.Count == 2
                               && room.ChatMembers
                                   .Select(member => member.ProfileId)
                                   .Contains(firstId)
                               && room.ChatMembers
                                   .Select(member => member.ProfileId)
                                   .Contains(secondId))
                .FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<ChatRoom>> AllAsync(Guid userId)
        {
            return (await RepoDbContext.ChatRooms
                    .Include(room => room.ChatMembers)
                    .Include(room => room.Messages)
                    .Where(room => room.ChatMembers
                        .Select(member => member.ProfileId)
                        .Contains(userId))
                    .ToListAsync())
                .Select(room => Mapper.Map(room));
        }
    }
}