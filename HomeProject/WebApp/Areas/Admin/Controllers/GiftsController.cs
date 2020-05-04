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
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class GiftsController : Controller
    {
        private readonly IAppBLL _bll;

        public GiftsController(IAppBLL bll, IWebHostEnvironment hostEnvironment)
        {
            _bll = bll;
            _bll.Images.RootPath = hostEnvironment.WebRootPath;
        }

        // GET: Gifts
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Gifts.AllAsync());
        }

        // GET: Gifts/Details/5
        public async Task<IActionResult> Details(Guid id)
        {

            var gift = await _bll.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
        }

        // GET: Gifts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gifts/Create
        // To protect from overgifting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gift gift)
        {
            if (gift.GiftImage.ImageFile == null)
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

        // GET: Gifts/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {

            var gift = await _bll.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }
            
            return View(gift);
        }

        // POST: Gifts/Edit/5
        // To protect from overgifting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Gift gift)
        {
            if (id != gift.Id)
            {
                return NotFound();
            }

            if (gift.GiftImage.ImageFile == null && gift.GiftImageId == null)
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

        // GET: Gifts/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var gift = await _bll.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
        }

        // POST: Gifts/Delete/5
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
