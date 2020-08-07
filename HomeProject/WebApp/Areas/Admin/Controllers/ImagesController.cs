using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Images - avatar, post, gifts etc...
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class ImagesController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        /// <param name="hostEnvironment"></param>
        public ImagesController(IAppBLL bll, IWebHostEnvironment hostEnvironment)
        {
            _bll = bll;
            _bll.Images.RootPath = hostEnvironment.WebRootPath;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Images.AllAdminAsync());
        }

        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.Images.GetRecordHistoryAsync(id)).ToList()
                ;

            return View(nameof(Index), history);
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var image = await _bll.Images.FindAdminAsync(id);
            if (image == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(image);
        }

        /// <summary>
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Load image, validate it, and save to localstorage
        /// </summary>
        /// <param name="imageModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(104857600)]
        public async Task<IActionResult> Create(Image imageModel)
        {
            if (imageModel.ImageFile == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ImageRequired);
                return View(imageModel);
            }

            if (imageModel.ImageType != ImageType.Undefined && imageModel.ImageFor == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ErrorForIdRequired);
                return View(imageModel);
            }

            ModelState.Clear();

            var (result, errors) = _bll.Images.ValidateImage(imageModel);

            if (errors.Length > 0)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            if (result != null && ModelState.IsValid)
            {
                result.Id = Guid.NewGuid();
                switch (result.ImageType)
                {
                    case ImageType.ProfileAvatar:
                        await _bll.Images.AddProfileAsync(result.ImageFor ?? Guid.Empty, result);
                        break;
                    case ImageType.Post:
                        await _bll.Images.AddPostAsync(result.ImageFor ?? Guid.Empty, result);
                        break;
                    case ImageType.Gift:
                        await _bll.Images.AddGiftAsync(result.ImageFor ?? Guid.Empty, result);
                        break;
                    default:
                        await _bll.Images.AddUndefinedAsync(result);
                        break;
                }

                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(imageModel);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var image = await _bll.Images.FindAdminAsync(id);
            if (image == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(image);
        }

        /// <summary>
        /// Edit images, and save as new image
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imageModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(104857600)]
        public async Task<IActionResult> Edit(Guid id, Image imageModel)
        {
            if (id != imageModel.Id)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorIdMatch);
                return View(imageModel);
            }

            if (imageModel.ImageType != ImageType.Undefined && imageModel.ImageFor == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ErrorForIdRequired);
                return View(imageModel);
            }

            Image? result;

            if (imageModel.ImageFile != null)
            {
                string[] errors;
                (result, errors) = _bll.Images.ValidateImage(imageModel);

                if (errors.Length > 0)
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }
            else
            {
                result = imageModel;
            }

            if (result != null && ModelState.IsValid)
            {
                switch (result.ImageType)
                {
                    case ImageType.ProfileAvatar:
                        await _bll.Images.UpdateProfileAsync(result.ImageFor ?? Guid.Empty, result);
                        break;
                    case ImageType.Post:
                        await _bll.Images.UpdatePostAsync(result.ImageFor ?? Guid.Empty, result);
                        break;
                    case ImageType.Gift:
                        await _bll.Images.UpdateGiftAsync(result.ImageFor ?? Guid.Empty, result);
                        break;
                    default:
                        await _bll.Images.UpdateUndefinedAsync(result);
                        break;
                }

                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(imageModel);
        }

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var image = await _bll.Images.FindAdminAsync(id);
            _bll.Images.Remove(image);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Restores a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.Images.GetForUpdateAsync(id);
            _bll.Images.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}