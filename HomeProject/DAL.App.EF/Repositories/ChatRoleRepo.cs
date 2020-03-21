using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ChatRoleRepo : BaseRepo<ChatRole>, IChatRoleRepo
    {
        public ChatRoleRepo(DbContext dbContext) : base(dbContext)
        {
        }
    }
}