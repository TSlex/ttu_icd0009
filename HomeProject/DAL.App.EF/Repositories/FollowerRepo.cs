using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class FollowerRepo : BaseRepo<Follower, ApplicationDbContext>, IFollowerRepo
    {
        public FollowerRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}