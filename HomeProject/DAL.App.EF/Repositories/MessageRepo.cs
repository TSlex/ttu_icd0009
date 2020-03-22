using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class MessageRepo : BaseRepo<Message, ApplicationDbContext>, IMessageRepo
    {
        public MessageRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}