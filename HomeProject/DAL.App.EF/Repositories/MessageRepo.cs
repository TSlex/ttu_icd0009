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
    public class MessageRepo : BaseRepo<Domain.Message, Message, ApplicationDbContext>, IMessageRepo
    {
        public MessageRepo(ApplicationDbContext dbContext) :
            base(dbContext, new UniversalDALMapper<Domain.Message, Message>())
        {
        }

        public override async Task<Message> FindAsync(Guid id)
        {
            return Mapper.Map(await GetQuery()
                .FirstOrDefaultAsync(message => message.Id == id));
        }

        public override Message Add(Message entity)
        {
            var members = RepoDbContext.ChatMembers
                .Where(member =>
                    member.ChatRoomId == entity.ChatRoomId
                    && member.MasterId == null)
                .ToList();

            foreach (var member in members)
            {
                if (member?.DeletedAt != null)
                {
                    member.DeletedAt = null;
                    member.DeletedBy = null;

                    RepoDbContext.ChatMembers.Update(member);
                }
            }

            return base.Add(entity);
        }

        public async Task<IEnumerable<Message>> AllAsync(Guid id)
        {
            return await RepoDbContext.Messages
                .Where(message =>
                    message.ChatRoomId == id &&
                    message.DeletedAt == null)
                .Select(message => Mapper.Map(message))
                .ToListAsync();
        }

        public async Task<int> CountByRoomAsync(Guid chatRoomId)
        {
            return await RepoDbContext.Messages.Where(message => message.DeletedAt == null)
                .CountAsync(message => message.ChatRoomId == chatRoomId);
        }

        public async Task<IEnumerable<Message>> AllByIdPageAsync(Guid chatRoomId, int pageNumber, int count)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * count;

            if (pageIndex < 0)
            {
                return new Message[] { };
            }

            return (await GetQuery()
                    .Where(message =>
                        message.ChatRoomId == chatRoomId &&
                        message.DeletedAt == null)
                    .OrderByDescending(message => message.MessageDateTime)
                    .Skip(startIndex)
                    .Take(count)
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }

        public async Task<Message> GetLastMessage(Guid chatRoomId)
        {
            return Mapper.Map(await GetQuery()
                .Where(message =>
                    message.ChatRoomId == chatRoomId &&
                    message.DeletedAt == null)
                .OrderByDescending(message => message.MessageDateTime)
                .Take(1)
                .FirstOrDefaultAsync());
        }

        public override async Task<IEnumerable<Message>> GetRecordHistoryAsync(Guid id)
        {
            return (await RepoDbSet
                    .Where(record => record.Id == id || record.MasterId == id)
                    .ToListAsync())
                .Select(record => Mapper.Map(record));
        }

        private IQueryable<Domain.Message> GetQuery()
        {
            return RepoDbSet
                .Include(message => message.Profile)
                .AsQueryable();
        }
    }
}