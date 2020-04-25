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
    }
}