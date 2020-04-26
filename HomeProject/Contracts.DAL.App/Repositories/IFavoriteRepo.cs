using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IFavoriteRepo : IBaseRepo<Favorite>
    {
        Task<Favorite> FindAsync(Guid id, Guid userId);
    }
}