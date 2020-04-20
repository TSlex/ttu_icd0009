using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ChatRoleRepo : BaseRepo<ChatRole, ApplicationDbContext>, IChatRoleRepo
    {
        public ChatRoleRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}