﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using BLL.App.Mappers;
using ee.itcollege.aleksi.BLL.Base.Mappers;
using ee.itcollege.aleksi.BLL.Base.Services;
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
            base(uow.Images, new UniversalBLLMapper<DAL.App.DTO.Image, Image>())
        {
        }

        private static string FormatPath(string? path)
        {
            return (path ?? "").Replace(@"\", Path.DirectorySeparatorChar.ToString())
                .Replace(@"/", Path.DirectorySeparatorChar.ToString()).Trim(Path.DirectorySeparatorChar);
        }

        public string GetImageUndefinedPath()
        {
            return FormatPath("~/localstorage/images/misc/404.png");
        }

        public string GetImagePath(string? path)
        {
            return FormatPath("~/localstorage" + (path ?? ""));
        }

        public bool ImagePathExists(string? path)
        {
            return File.Exists(Path.Combine(RootPath, "localstorage", FormatPath(path ?? "")));
        }

        public Tuple<Image?, string[]> ValidateImage(Image imageModel)
        {
            var errors = new List<string>();

            if (imageModel.ImageFile == null)
            {
                errors.Add(Resourses.BLL.App.DTO.Images.Images.ImageRequired);
                return new Tuple<Image?, string[]>(null, errors.ToArray());
            }

            var extension = Path.GetExtension(imageModel.ImageFile!.FileName)?.ToLower();

            if (!(extension == ".png" || extension == ".jpg" || extension == ".jpeg"))
            {
                errors.Add(Resourses.BLL.App.DTO.Images.Images.ExtensionsSupported);
                return new Tuple<Image?, string[]>(null, errors.ToArray());
            }

            using (var image = System.Drawing.Image.FromStream(imageModel.ImageFile.OpenReadStream()))
            {
                if (image.Height > 4000 || image.Width > 4000)
                {
                    errors.Add(Resourses.BLL.App.DTO.Images.Images.ErrorMaxImageResolution);
                    return new Tuple<Image?, string[]>(null, errors.ToArray());
                }

                var ratio = image.Height * 1.0 / image.Width;
                if (ratio < 0.1 || 10 < ratio)
                {
                    errors.Add(Resourses.BLL.App.DTO.Images.Images.ErrorImageSupportedRatio);
                    return new Tuple<Image?, string[]>(null, errors.ToArray());
                }

                imageModel.HeightPx = image.Height;
                imageModel.WidthPx = image.Width;
            }

            return new Tuple<Image?, string[]>(imageModel, errors.ToArray());
        }

        public async Task<Image> FindProfileAsync(Guid userId)
        {
            return Mapper.Map(await ServiceRepository.FindProfileAsync(userId));
        }

        public async Task<Image> AddProfileAsync(Guid profileId, Image entity)
        {
//            string folderPath = RootPath + $"\\localstorage\\images\\profiles\\{profileId.ToString()}\\";

            var folderPath = Path.Combine(RootPath, "localstorage", "images", "profiles", profileId.ToString());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

//            FileInfo directory = new FileInfo(folderPath);
//            directory.Directory?.Create();

            return await AddAsync(entity, folderPath);
        }

        public async Task<Image> UpdateProfileAsync(Guid profileId, Image entity)
        {
//            string folderPath = RootPath + $"\\localstorage\\images\\profiles\\{profileId.ToString()}\\";

            var folderPath = Path.Combine(RootPath, "localstorage", "images", "profiles", profileId.ToString());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

//            FileInfo directory = new FileInfo(folderPath);
//            directory.Directory?.Create();

            return await UpdateAsync(entity, folderPath);
        }

        public async Task<Image> FindPostAsync(Guid postId)
        {
            return Mapper.Map(await ServiceRepository.FindPostAsync(postId));
        }

        public async Task<Image> AddPostAsync(Guid postId, Image entity)
        {
//            string folderPath = RootPath + $"\\localstorage\\images\\posts\\{postId.ToString()}\\";

            var folderPath = Path.Combine(RootPath, "localstorage", "images", "posts", postId.ToString());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

//            FileInfo directory = new FileInfo(folderPath);
//            directory.Directory?.Create();

            return await AddAsync(entity, folderPath);
        }

        public async Task<Image> UpdatePostAsync(Guid postId, Image entity)
        {
//            string folderPath = RootPath + $"\\localstorage\\images\\posts\\{postId.ToString()}\\";

            var folderPath = Path.Combine(RootPath, "localstorage", "images", "posts", postId.ToString());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

//            FileInfo directory = new FileInfo(folderPath);
//            directory.Directory?.Create();

            return await UpdateAsync(entity, folderPath);
        }

        public async Task<Image> FindGiftAsync(Guid giftId)
        {
            return Mapper.Map(await ServiceRepository.FindPostAsync(giftId));
        }

        public async Task<Image> AddGiftAsync(Guid giftId, Image entity)
        {
//            string folderPath = RootPath + $"\\localstorage\\images\\gifts\\{giftId.ToString()}\\";

            var folderPath = Path.Combine(RootPath, "localstorage", "images", "gifts", giftId.ToString());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

//            FileInfo directory = new FileInfo(folderPath);
//            directory.Directory?.Create();

            return await AddAsync(entity, folderPath);
        }

        public async Task<Image> UpdateGiftAsync(Guid giftId, Image entity)
        {
//            string folderPath = RootPath + $"\\localstorage\\images\\gifts\\{giftId.ToString()}\\";

            var folderPath = Path.Combine(RootPath, "localstorage", "images", "gifts", giftId.ToString());

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

//            FileInfo directory = new FileInfo(folderPath);
//            directory.Directory?.Create();

            return await UpdateAsync(entity, folderPath);
        }

        public async Task<Image> AddUndefinedAsync(Image entity)
        {
//            string folderPath = RootPath + $"\\localstorage\\images\\misc\\";

            var folderPath = Path.Combine(RootPath, "localstorage", "images", "misc");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

//            FileInfo directory = new FileInfo(folderPath);
//            directory.Directory?.Create();

            return await AddAsync(entity, folderPath);
        }

        public async Task<Image> UpdateUndefinedAsync(Image entity)
        {
//            string folderPath = RootPath + $"\\localstorage\\images\\misc\\";

            var folderPath = Path.Combine(RootPath, "localstorage", "images", "misc");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

//            FileInfo directory = new FileInfo(folderPath);
//            directory.Directory?.Create();

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
//                return await UpdateAsync(entity);
            }

            else if ((entity.PaddingTop != record.PaddingTop || entity.PaddingRight != record.PaddingRight ||
                      entity.PaddingBottom != record.PaddingBottom || entity.PaddingLeft != record.PaddingLeft) &&
                     (entity.PaddingTop != 0 || entity.PaddingRight != 0 ||
                      entity.PaddingBottom != 0 || entity.PaddingLeft != 0))
            {
                //save new file
                var filename = Guid.NewGuid() + "." + entity.OriginalImageUrl!.Split('.')[1];
                filename = Path.Combine(path, FormatPath(filename));
                entity.ImageUrl = filename.Split("localstorage")[1];

                Bitmap newImage;

//                var filePath = RootPath + "\\localstorage\\" + entity.OriginalImageUrl;
                var filePath = Path.Combine(RootPath, "localstorage", FormatPath(entity.OriginalImageUrl));

                await using (var file = File.OpenRead(filePath))
                {
                    newImage = CropImageStreamByPaddings(file, entity.PaddingTop,
                        entity.PaddingRight, entity.PaddingBottom, entity.PaddingLeft, entity.WidthPx, entity.HeightPx);
                }

                await using (var stream = File.Create(filename))
                {
                    newImage.Save(stream, ImageFormat.Png);
                }

//                return await UpdateAsync(entity);
            }

            else if (entity.PaddingTop == 0 && entity.PaddingRight == 0 &&
                     entity.PaddingBottom == 0 && entity.PaddingLeft == 0)
            {
                entity.ImageUrl = entity.OriginalImageUrl;
//                return await UpdateAsync(entity);
            }

            return await UpdateAsync(entity);
//            return entity;
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

            originalFilename = Path.Combine(path, FormatPath(originalFilename));
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
                filename = Path.Combine(path, FormatPath(filename));
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