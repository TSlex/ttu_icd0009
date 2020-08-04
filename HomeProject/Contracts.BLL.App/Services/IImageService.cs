using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IImageService: IBaseEntityService<global::DAL.App.DTO.Image, Image>
    {
        string RootPath { get; set; }

        Tuple<Image?, string[]> ValidateImage(Image imageModel);
        
        Task<Image> AddProfileAsync(Guid profileId, Image entity);
        Task<Image> UpdateProfileAsync(Guid profileId, Image entity);

        Task<Image> AddPostAsync(Guid postId, Image entity);
        Task<Image> UpdatePostAsync(Guid postId, Image entity);
        
        Task<Image> AddGiftAsync(Guid giftId, Image entity);
        Task<Image> UpdateGiftAsync(Guid giftId, Image entity);

        Task<Image> AddUndefinedAsync(Image entity);
        Task<Image> UpdateUndefinedAsync(Image entity);
        
        Task<Image> FindProfileAsync(Guid userId);
        Task<Image> FindPostAsync(Guid userId);
        Task<Image> FindGiftAsync(Guid userId);
    }
}