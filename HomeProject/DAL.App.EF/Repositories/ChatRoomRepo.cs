using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.aleksi.DAL.Base.EF.Repositories;
using DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ChatRoomRepo : BaseRepo<Domain.ChatRoom, ChatRoom, ApplicationDbContext>, IChatRoomRepo
    {
        public ChatRoomRepo(ApplicationDbContext dbContext) :
            base(dbContext, new UniversalDALMapper<Domain.ChatRoom, ChatRoom>())
        {
        }

        public override async Task<ChatRoom> FindAdminAsync(Guid id)
        {
            var raw = await GetQuery()
                .Where(room => room.Id == id)
                .Select(room => new
                {
                    value = room,
                    lastMessage = room.Messages.OrderByDescending(message => message.MessageDateTime).FirstOrDefault()
                }).FirstOrDefaultAsync();

            var record = Mapper.Map(raw.value);

            if (record == null || raw.lastMessage == null) return Mapper.Map(raw.value);

            record.LastMessageValue = raw.lastMessage.MessageValue;
            record.LastMessageDateTime = raw.lastMessage.MessageDateTime;
            record.ChatRoomImageId = raw.lastMessage.Profile!.ProfileAvatarId;

            return record;
        }

        public override async Task<ChatRoom> FindAsync(Guid id)
        {
            return Mapper.Map(await GetQuery()
                .Include(room => room.ChatMembers)
                .ThenInclude(member => member.ChatRole)
                .ThenInclude(role => role!.RoleTitleValue)
                .ThenInclude(s => s!.Translations)
                .FirstOrDefaultAsync(room => room.Id == id));
        }

#pragma warning disable 8604
        public async Task<ChatRoom> GetRoomWithUserAsync(Guid userId, Guid requesterId)
        {
            var chatRoom = await RepoDbContext.ChatRooms
                .Include(room => room.ChatMembers)
                .Where(room => room.ChatMembers!.Count == 2
                               && room.MasterId == null
                               && room.ChatMembers
                                   .Select(member => member.ProfileId)
                                   .Contains(userId)
                               && room.ChatMembers
                                   .Select(member => member.ProfileId)
                                   .Contains(requesterId))
                .FirstOrDefaultAsync();

            if (chatRoom?.DeletedAt != null)
            {
                chatRoom.DeletedAt = null;
                chatRoom.DeletedBy = null;

                RepoDbSet.Update(chatRoom);
                await RepoDbContext.SaveChangesAsync();
            }

            var member = chatRoom?.ChatMembers.FirstOrDefault(chatMember => chatMember.ProfileId == requesterId);

            if (member != null && member.DeletedAt != null)
            {
                member.DeletedAt = null;
                member.DeletedBy = null;

                RepoDbContext.ChatMembers.Update(member);
                await RepoDbContext.SaveChangesAsync();
            }

            return Mapper.Map(chatRoom);
        }
#pragma warning restore 8604

        public async Task<IEnumerable<ChatRoom>> AllAsync(Guid userId)
        {
            return (await GetQuery()
                    .Include(room => room.ChatMembers)
                    .Where(room => room.ChatMembers
                                       .Select(member => member.ProfileId)
                                       .Contains(userId)
                                   && room.DeletedAt == null)
                    .Select(room => new Domain.ChatRoom()
                    {
                        Id = room.Id,
                        ChatMembers = room.ChatMembers,
                        ChatRoomTitle = room.ChatRoomTitle,
                        Messages = room.Messages.OrderByDescending(message => message.MessageDateTime)
                            .Where(message => message.DeletedAt == null && message.MasterId == null)
                            .Take(1).ToList()
                    })
                    .ToListAsync())
                .Select(room => Mapper.Map(room));
        }

        public async Task<bool> IsRoomMemberAsync(Guid chatRoomId, Guid userId)
        {
            return (await RepoDbContext.ChatRooms.FirstOrDefaultAsync(room =>
                       room.Id == chatRoomId &&
                       room.ChatMembers.Select(member => member.ProfileId).Contains(userId))) != null;
        }

        public async Task<bool> IsRoomAdministratorAsync(Guid chatRoomId, Guid userId)
        {
            return (await RepoDbContext.ChatRooms.FirstOrDefaultAsync((room =>
                           room.Id == chatRoomId &&
                           room.ChatMembers.Select(member => member.ProfileId).Contains(userId) &&
                           (
                               room.ChatMembers.Select(member => member.ChatRole!.RoleTitle).Contains("Creator") ||
                               room.ChatMembers.Select(member => member.ChatRole!.RoleTitle).Contains("Administrator")
                           )
                       ))) != null;
        }

        public override ChatRoom Remove(ChatRoom tEntity)
        {
            var members = RepoDbContext.ChatMembers.Where(member => member.ChatRoomId == tEntity.Id).ToList();

            foreach (var chatMember in members)
            {
                RepoDbContext.ChatMembers.Remove(chatMember);
            }

            var messages = RepoDbContext.Messages.Where(message => message.ChatRoomId == tEntity.Id).ToList();

            foreach (var message in messages)
            {
                RepoDbContext.Messages.Remove(message);
            }

            return base.Remove(tEntity);
        }

        public override async Task<IEnumerable<ChatRoom>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet.Where(record => record.Id == id || record.MasterId == id).ToListAsync()).Select(
                record => Mapper.Map(record));
        }
        
        private IQueryable<Domain.ChatRoom> GetQuery()
        {
            return RepoDbSet
                .Include(room => room.Messages)
                .ThenInclude(message => message.Profile)
                .AsQueryable();
        }
    }
}