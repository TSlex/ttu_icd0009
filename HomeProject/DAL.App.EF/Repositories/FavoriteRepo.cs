using Contracts.DAL.App.Repositories;
using DAL.Base.EF.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class FavoriteRepo : BaseRepo<Favorite, ApplicationDbContext>, IFavoriteRepo
    {
        public FavoriteRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}