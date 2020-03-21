using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GiftRepo : BaseRepo<Gift>, IGiftRepo
    {
        public GiftRepo(DbContext dbContext) : base(dbContext)
        {
        }
    }
}