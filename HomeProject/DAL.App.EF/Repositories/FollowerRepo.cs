using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base.EF.Mappers;
using DAL.Base.EF.Repositories;

namespace DAL.Repositories
{
    public class FollowerRepo : BaseRepo<Domain.Follower, Follower, ApplicationDbContext>, IFollowerRepo
    {
        public FollowerRepo(ApplicationDbContext dbContext) : 
            base(dbContext, new BaseDALMapper<Domain.Follower, Follower>())
        {
        }
    }
}