using System;
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
            return View(await _bll.Gifts.AllAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {

            var gift = await _bll.Gifts.FindAsync(id);

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
        public async Task<IActionResult> Create(Gift gift)
        {
            if (gift.GiftImage!.ImageFile == null)
            {
                ModelState.AddModelError(string.Empty, "Image should be specified");
                return View(gift);
            }
            
            ModelState.Clear();

            if (TryValidateModel(gift))
            {
                gift.Id = Guid.NewGuid();
                
                var imageModel = gift.GiftImage;
                imageModel.Id = Guid.NewGuid();
                imageModel.ImageType = ImageType.Gift;
                imageModel.ImageFor = gift.Id;

                gift.GiftImageId = imageModel.Id;
                gift.GiftImage = null;
                    
                _bll.Gifts.Add(gift);
                await _bll.Images.AddGiftAsync(gift.Id, imageModel);
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

            var gift = await _bll.Gifts.FindAsync(id);

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
        public async Task<IActionResult> Edit(Guid id, Gift gift)
        {
            if (id != gift.Id)
            {
                return NotFound();
            }

            if (gift.GiftImage!.ImageFile == null && gift.GiftImageId == null)
            {
                ModelState.AddModelError(string.Empty, "Image should be specified");
                return View(gift);
            }
            
            ModelState.Clear();

            var imageModel = gift.GiftImage;
            imageModel.ImageType = ImageType.Gift;
            imageModel.ImageFor = gift.Id;

            if (TryValidateModel(gift))
            {
                gift.GiftImage = null;
                gift.GiftImageId = imageModel.Id;

                await _bll.Images.UpdateGiftAsync(gift.Id, imageModel);
                await _bll.Gifts.UpdateAsync(gift);
                await _bll.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            return View(gift);
        }

        /// <summary>
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {

            var gift = await _bll.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
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
    }
}
