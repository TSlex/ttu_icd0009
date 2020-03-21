using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProfileRankRepo : BaseRepo<ProfileRank>, IProfileRankRepo
    {
        public ProfileRankRepo(DbContext dbContext) : base(dbContext)
        {
        }
    }
}