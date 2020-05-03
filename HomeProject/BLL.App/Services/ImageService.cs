using System;
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
        public string RootPath { get; set; }
        
        public ImageService(IAppUnitOfWork uow) :
            base(uow.Images, new BaseBLLMapper<DAL.App.DTO.Image, Image>())
        {
        }

        public async Task<Image> AddProfileAsync(Guid profileId, Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\profiles\\{profileId.ToString()}\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return await AddAsync(entity, folderPath);
        }
        
        public async Task<Image> UpdateProfileAsync(Guid profileId, Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\profiles\\{profileId.ToString()}\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return await UpdateAsync(entity, folderPath);
        }
        
        public Image RemoveProfile(Guid profileId, Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\profiles\\{profileId.ToString()}\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return Remove(entity, folderPath);
        }
        
        public async Task<Image> AddPostAsync(Guid postId, Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\posts\\{postId.ToString()}\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return await AddAsync(entity, folderPath);
        }
        
        public async Task<Image> UpdatePostAsync(Guid postId, Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\posts\\{postId.ToString()}\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return await UpdateAsync(entity, folderPath);
        }
        
        public Image RemovePost(Guid postId, Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\posts\\{postId.ToString()}\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return Remove(entity, folderPath);
        }
        
        public async Task<Image> AddGiftAsync(Guid giftId, Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\gifts\\{giftId.ToString()}\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return await AddAsync(entity, folderPath);
        }
        
        public async Task<Image> UpdateGiftAsync(Guid giftId, Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\gifts\\{giftId.ToString()}\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return await UpdateAsync(entity, folderPath);
        }
        
        public Image RemoveGift(Guid giftId, Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\gifts\\{giftId.ToString()}\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return Remove(entity, folderPath);
        }
        
        public async Task<Image> AddUndefinedAsync(Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\misc\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return await AddAsync(entity, folderPath);
        }
        
        public async Task<Image> UpdateUndefinedAsync(Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\misc\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return await UpdateAsync(entity, folderPath);
        }
        
        public Image RemoveUndefined(Image entity)
        {
            string folderPath = RootPath + $"\\localstorage\\images\\misc\\";
            
            FileInfo directory = new FileInfo(folderPath);
            directory.Directory?.Create();

            return Remove(entity, folderPath);
        }
        
        
        //method for image crud
        private async Task<Image> AddAsync(Image entity, string path)
        {
            string filename = entity.Id.ToString();
            string extension = Path.GetExtension(entity.ImageFile.FileName);
            filename = filename + extension;
            
            filename = Path.Combine(path, filename);
            entity.ImageUrl = filename.Split("localstorage")[1];

            using (var stream = File.Create(filename))
            {
                await entity.ImageFile.CopyToAsync(stream);
            }

            return base.Add(entity);
        }

        private async Task<Image> UpdateAsync(Image entity, string path)
        {
            if (entity.ImageFile != null)
            {
                string filename = entity.Id.ToString();
                string extension = Path.GetExtension(entity.ImageFile.FileName);
                filename = filename + extension;

                filename = Path.Combine(path, filename);
                entity.ImageUrl = filename.Split("localstorage")[1];

                using (var stream = File.OpenWrite(filename))
                {
                    await entity.ImageFile.CopyToAsync(stream);
                }
            }

            return await base.UpdateAsync(entity);
        }

        private Image Remove(Image entity, string path)
        {
            var filename = path.Split("localstorage")[0] + "localstorage" + entity.ImageUrl;

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            return base.Remove(entity);
        }
    }
}