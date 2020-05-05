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
            base(dbContext, new MessageMapper())
        {
        }

        public async Task<IEnumerable<Message>> AllAsync(Guid id)
        {
            return await RepoDbContext.Messages.Where(message => message.ChatRoomId == id)
                .Select(message => Mapper.Map(message)).ToListAsync();
        }

        public async Task<IEnumerable<Message>> AllByIdPageAsync(Guid chatRoomId, int pageNumber, int count)
        {
            var pageIndex = pageNumber - 1;
            var startIndex = pageIndex * count;

            if (pageIndex < 0)
            {
                return new Message[] { };
            }

            return (await RepoDbContext.Messages
                    .Where(message => message.ChatRoomId == chatRoomId)
                    .Include(message => message.Profile)
                    .OrderBy(message => message.MessageDateTime)
                    .Skip(startIndex)
                    .Take(count)
                    .Select(message => new Domain.Message()
                    {
                        Id = message.Id,
                        MessageValue = message.MessageValue,
                        Profile = new Domain.Profile()
                        {
                            UserName = message.Profile.UserName
                        },
                        MessageDateTime = message.MessageDateTime
                    })
                    .ToListAsync())
                .Select(post => Mapper.Map(post));
        }

        public async Task<int> CountByRoomAsync(Guid chatRoomId)
        {
            return await RepoDbContext.Messages.CountAsync(message => message.ChatRoomId == chatRoomId);
        }

        public async Task<Message> GetLastMessage(Guid chatRoomId)
        {
            return Mapper.Map(await RepoDbContext.Messages
                .Where(message => message.ChatRoomId == chatRoomId)
                .Include(message => message.Profile)
                .OrderByDescending(message => message.MessageDateTime)
                .Take(1)
                .Select(message => new Domain.Message()
                {
                    Id = message.Id,
                    MessageValue = message.MessageValue,
                    Profile = new Domain.Profile()
                    {
                        UserName = message.Profile.UserName
                    },
                    MessageDateTime = message.MessageDateTime
                })
                .FirstOrDefaultAsync());
        }
    }
}