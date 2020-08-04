using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using ee.itcollege.aleksi.Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IFavoriteService: IBaseEntityService<global::DAL.App.DTO.Favorite, Favorite>
    {
        Task<Favorite> FindAsync(Guid id, Guid userId);
        Favorite? Create(Guid id, Guid userId);
        Task<Favorite> RemoveAsync(Guid id, Guid userId);

        Task<IEnumerable<Favorite>> AllByPostIdPageAsync(Guid postId, int pageNumber, int i);
        Task<int> CountByIdAsync(Guid postId);
    }
}