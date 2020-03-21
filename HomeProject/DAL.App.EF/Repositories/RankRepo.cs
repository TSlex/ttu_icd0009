using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class RankRepo : BaseRepo<Rank>, IRankRepo
    {
        public RankRepo(DbContext dbContext) : base(dbContext)
        {
        }
    }
}