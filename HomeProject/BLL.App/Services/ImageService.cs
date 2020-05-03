using System.IO;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.Base.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;

namespace BLL.App.Services
{
    public class ImageService : BaseEntityService<IImageRepo, DAL.App.DTO.Image, Image>, IImageService
    {
        public ImageService(IAppUnitOfWork uow) :
            base(uow.Images, new BaseBLLMapper<DAL.App.DTO.Image, Image>())
        {
        }

        public async Task<Image> AddAsync(Image entity, string rootPath)
        {
            string folderPath = rootPath + "\\localstorage\\images";

            string filename = entity.Id.ToString();
            string extension = Path.GetExtension(entity.ImageFile.FileName);
            filename = filename + extension;

            entity.ImageUrl = "/images/" + filename;

            filename = Path.Combine(folderPath, filename);

            using (var stream = File.Create(filename))
            {
                await entity.ImageFile.CopyToAsync(stream);
            }

            return base.Add(entity);
        }

        public async Task<Image> UpdateAsync(Image entity, string rootPath)
        {
            if (entity.ImageFile != null)
            {
                string folderPath = rootPath + "\\localstorage\\images";

                string filename = entity.Id.ToString();
                string extension = Path.GetExtension(entity.ImageFile.FileName);
                filename = filename + extension;

                entity.ImageUrl = "/images/" + filename;

                filename = Path.Combine(folderPath, filename);

                using (var stream = File.OpenWrite(filename))
                {
                    await entity.ImageFile.CopyToAsync(stream);
                }
            }

            return await base.UpdateAsync(entity);
        }

        public Image Remove(Image entity, string rootPath)
        {
            string folderPath = rootPath + "\\localstorage";
            
            var filename = folderPath + entity.ImageUrl;

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            return base.Remove(entity);
        }
    }
}