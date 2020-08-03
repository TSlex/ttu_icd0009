using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Domain.Enums;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Gifts
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class GiftsController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Controller
        /// </summary>
        /// <param name="bll"></param>
        /// <param name="hostEnvironment"></param>
        public GiftsController(IAppBLL bll, IWebHostEnvironment hostEnvironment)
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
            return View(await _bll.Gifts.AllAdminAsync());
        }

        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.Gifts.GetRecordHistoryAsync(id)).ToList()
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
            var gift = await _bll.Gifts.FindAdminAsync(id);

            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
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
        /// Creates a new record
        /// </summary>
        /// <param name="gift"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(104857600)] 
        public async Task<IActionResult> Create(Gift gift)
        {
            if (gift.GiftImage!.ImageFile == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ImageRequired);
                return View(gift);
            }

            var (imageModel, errors) = _bll.Images.ValidateImage(gift.GiftImage);

            if (errors.Length > 0)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            if (TryValidateModel(gift))
            {
                gift.Id = Guid.NewGuid();

                imageModel.Id = Guid.NewGuid();
                imageModel.ImageType = ImageType.Gift;
                imageModel.ImageFor = gift.Id;

                gift.GiftImageId = imageModel.Id;
                gift.GiftImage = null;

                await _bll.Images.AddGiftAsync(gift.Id, imageModel);
                _bll.Gifts.Add(gift);

                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(gift);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var gift = await _bll.Gifts.FindAdminAsync(id);

            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gift"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(104857600)]
        public async Task<IActionResult> Edit(Guid id, Gift gift)
        {
            if (id != gift.Id)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorIdMatch);
                return View(gift);
            }

            if (gift.GiftImage!.ImageFile == null && gift.GiftImageId == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ImageRequired);
                return View(gift);
            }

            ModelState.Clear();

            var imageModel = gift.GiftImage;

            if (gift.GiftImage.ImageFile != null)
            {
                var (_, errors) = _bll.Images.ValidateImage(gift.GiftImage);

                if (errors.Length > 0)
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }

            if (gift.GiftImageId == null)
            {
                imageModel.Id = Guid.NewGuid();
                imageModel.ImageType = ImageType.Gift;
                imageModel.ImageFor = gift.Id;
            }

            if (TryValidateModel(gift))
            {
                if (gift.GiftImageId == null)
                {
                    await _bll.Images.AddGiftAsync(gift.Id, imageModel);
                }
                else
                {
                    await _bll.Images.UpdateGiftAsync(gift.Id, imageModel);
                }

                gift.GiftImageId = imageModel.Id;
                gift.GiftImage = null;

                await _bll.Gifts.UpdateAsync(gift);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(gift);
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
            _bll.Gifts.Remove(id);
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
            var record = await _bll.Gifts.GetForUpdateAsync(id);
            _bll.Gifts.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}