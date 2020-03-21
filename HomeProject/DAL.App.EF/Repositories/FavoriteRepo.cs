using Contracts.DAL.App.Repositories;
using DAL.Base.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class FavoriteRepo : BaseRepo<Favorite>, IFavoriteRepo
    {
        public FavoriteRepo(DbContext dbContext) : base(dbContext)
        {
        }
    }
}