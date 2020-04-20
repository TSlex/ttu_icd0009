using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ChatRoomRepo : BaseRepo<ChatRoom, ApplicationDbContext>, IChatRoomRepo
    {
        public ChatRoomRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}