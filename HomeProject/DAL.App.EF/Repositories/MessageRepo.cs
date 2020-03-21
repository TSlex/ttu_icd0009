using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class MessageRepo : BaseRepo<Message>, IMessageRepo
    {
        public MessageRepo(DbContext dbContext) : base(dbContext)
        {
        }
    }
}