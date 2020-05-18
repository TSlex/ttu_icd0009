using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using BLL.Base.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Domain;
using Microsoft.AspNetCore.Http;
using Image = BLL.App.DTO.Image;

namespace BLL.App.Services
{
    public class ImageService : BaseEntityService<IImageRepo, DAL.App.DTO.Image, Image>, IImageService
    {
        public string RootPath { get; set; } = default!;

        public ImageService(IAppUnitOfWork uow) :
            base(uow.Images, new BaseBLLMapper<DAL.App.DTO.Image, Image>())
        {
        }
        

        public async Task<Image> FindProfileAsync(Guid userId)
        {
            return Mapper.Map(await ServiceRepository.FindProfileAsync(userId));
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
        
        public async Task<Image> FindPostAsync(Guid postId)
        {
            return Mapper.Map(await ServiceRepository.FindPostAsync(postId));
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
        
        public async Task<Image> FindGiftAsync(Guid giftId)
        {
            return Mapper.Map(await ServiceRepository.FindPostAsync(giftId));
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

        //method for image crud
        private async Task<Image> AddAsync(Image entity, string path)
        {
            await CreateImagesAsync(entity, path, false);

            return base.Add(entity);
        }

        private async Task<Image> UpdateAsync(Image entity, string path)
        {
            var record = await FindAsync(entity.Id);

            if (entity.ImageFile != null)
            {
                await CreateImagesAsync(entity, path, true);
                return await UpdateAsync(entity);
            }

            if ((entity.PaddingTop != record.PaddingTop || entity.PaddingRight != record.PaddingRight ||
                      entity.PaddingBottom != record.PaddingBottom || entity.PaddingLeft != record.PaddingLeft) &&
                     (entity.PaddingTop != 0 || entity.PaddingRight != 0 ||
                      entity.PaddingBottom != 0 || entity.PaddingLeft != 0))
            {
                //save new file
                var filename = Guid.NewGuid() + "." + entity.OriginalImageUrl!.Split('.')[1];
                filename = Path.Combine(path, filename);
                entity.ImageUrl = filename.Split("localstorage")[1];

                Bitmap newImage;

                await using (var file = File.OpenRead(RootPath + "\\localstorage\\" + entity.OriginalImageUrl))
                {
                    newImage = CropImageStreamByPaddings(file, entity.PaddingTop,
                        entity.PaddingRight, entity.PaddingBottom, entity.PaddingLeft, entity.WidthPx, entity.HeightPx);
                }

                await using (var stream = File.Create(filename))
                {
                    newImage.Save(stream, ImageFormat.Png);
                }
                
                return await UpdateAsync(entity);
            }

            if (entity.PaddingTop == 0 && entity.PaddingRight == 0 &&
                     entity.PaddingBottom == 0 && entity.PaddingLeft == 0)
            {
                entity.ImageUrl = entity.OriginalImageUrl;
                return await UpdateAsync(entity);
            }

            return entity;
        }

        public override Image Remove(Image entity)
        {
            var filename = RootPath + "\\localstorage\\" + entity.ImageUrl;

//            if (File.Exists(filename))
//            {
//                File.Delete(filename);
//            }

            return base.Remove(entity);
        }

        private static async Task CreateImagesAsync(Image entity, string path, bool update)
        {
            var extension = Path.GetExtension(entity.ImageFile!.FileName);
            string originalFilename;

            //save original file
            if (update)
            {
                originalFilename = Guid.NewGuid() + extension;
            }
            else
            {
                originalFilename = entity.Id + extension;
            }

            originalFilename = Path.Combine(path, originalFilename);
            entity.OriginalImageUrl = originalFilename.Split("localstorage")[1];

            await using (var stream = File.Create(originalFilename))
            {
                await entity.ImageFile.CopyToAsync(stream);
            }

            if (entity.PaddingTop != 0 || entity.PaddingRight != 0 ||
                entity.PaddingBottom != 0 || entity.PaddingLeft != 0)
            {
                //save new file
                var filename = Guid.NewGuid() + extension;
                filename = Path.Combine(path, filename);
                entity.ImageUrl = filename.Split("localstorage")[1];

                var newImage = CropImageByPaddings(entity.ImageFile, entity.PaddingTop,
                    entity.PaddingRight, entity.PaddingBottom, entity.PaddingLeft, entity.WidthPx, entity.HeightPx);

                using (var stream = File.Create(filename))
                {
                    newImage.Save(stream, ImageFormat.Png);
                }
            }
            else
            {
                entity.ImageUrl = entity.OriginalImageUrl;
            }
        }

        private static Bitmap CropImageStreamByPaddings(Stream file, int pt, int pr, int pb, int pl, int width,
            int height)
        {
            return CropImage(file, pl, pt, width - pl - pr, height - pt - pb);
        }

        private static Bitmap CropImageByPaddings(IFormFile file, int pt, int pr, int pb, int pl, int width, int height)
        {
            return CropImage(file.OpenReadStream(), pl, pt, width - pl - pr, height - pt - pb);
        }

        private static Bitmap CropImage(Stream file, int startX, int startY, int width, int height)
        {
            var image = System.Drawing.Image.FromStream(file);
            var newImage = new Bitmap(width, height);

            var srcRect = new Rectangle(startX, startY, width, height);
            var destRect = new Rectangle(0, 0, width, height);

            using var g = Graphics.FromImage(newImage);
            g.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);

            return newImage;
        }
    }
}