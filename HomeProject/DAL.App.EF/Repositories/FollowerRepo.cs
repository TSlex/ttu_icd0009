using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
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