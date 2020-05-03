using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface IImageService: IBaseEntityService<global::DAL.App.DTO.Image, Image>
    {
        Task<Image> AddAsync(Image entity, string rootPath);
        Task<Image> UpdateAsync(Image entity, string rootPath);
        Image Remove(Image entity, string rootPath);
    }
}