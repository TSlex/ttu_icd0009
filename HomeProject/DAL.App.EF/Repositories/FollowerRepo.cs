using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class FollowerRepo : BaseRepo<Follower>, IFollowerRepo
    {
        public FollowerRepo(DbContext dbContext) : base(dbContext)
        {
        }
    }
}