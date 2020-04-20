using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GiftRepo : BaseRepo<Gift, ApplicationDbContext>, IGiftRepo
    {
        public GiftRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}