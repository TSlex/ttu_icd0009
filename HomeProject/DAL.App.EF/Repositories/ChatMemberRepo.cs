using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ChatMemberRepo : BaseRepo<ChatMember, ApplicationDbContext>, IChatMemberRepo
    {
        public ChatMemberRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}