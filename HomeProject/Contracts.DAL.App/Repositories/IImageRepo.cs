using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IImageRepo: IBaseRepo<Image>
    {
        Task<Image> FindProfileAsync(Guid userId);
        Task<Image> FindPostAsync(Guid postId);
        Task<Image> FindGiftAsync(Guid giftId);
    }
}