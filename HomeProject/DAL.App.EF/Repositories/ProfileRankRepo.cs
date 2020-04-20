using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProfileRankRepo : BaseRepo<ProfileRank, ApplicationDbContext>, IProfileRankRepo
    {
        public ProfileRankRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}