using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IFavoriteRepo : IBaseRepo<Favorite>
    {
        Task<Favorite> FindAsync(Guid id, Guid userId);
        Task<IEnumerable<Favorite>> AllByIdPageAsync(Guid postId, int pageNumber, int count);
        Task<int> CountByIdAsync(Guid postId);
    }
}