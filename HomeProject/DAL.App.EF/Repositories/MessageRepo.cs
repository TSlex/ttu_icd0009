using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;

namespace DAL.Repositories
{
    public class MessageRepo : BaseRepo<Domain.Message, Message, ApplicationDbContext>, IMessageRepo
    {
        public MessageRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new BaseDALMapper<Domain.Message, Message>())
        {
        }
    }
}