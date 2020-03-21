using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ChatMemberRepo : BaseRepo<ChatMember>, IChatMemberRepo
    {
        public ChatMemberRepo(DbContext dbContext) : base(dbContext)
        {
        }
    }
}