using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class RankRepo : BaseRepo<Rank, ApplicationDbContext>, IRankRepo
    {
        public RankRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}