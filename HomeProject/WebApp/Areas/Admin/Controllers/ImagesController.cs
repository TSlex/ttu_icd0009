using System;
using System.IO;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ImagesController : Controller
    {
        private readonly IAppBLL _bll;
        
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImagesController(IAppBLL bll, IWebHostEnvironment hostEnvironment)
        {
            _bll = bll;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Images.AllAsync());
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var image = await _bll.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(104857600)] 
        public async Task<IActionResult> Create(Image imageModel)
        {
            if (imageModel.ImageFile == null)
            {
                return View(imageModel);
            }
            
            ModelState.Clear();
            
            var result = ValidateImage(imageModel);
            
            if (result != null && ModelState.IsValid)
            {
                result.Id = Guid.NewGuid();
                await _bll.Images.AddAsync(result, _hostEnvironment.WebRootPath);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(imageModel);
        }

        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var image = await _bll.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(104857600)] 
        public async Task<IActionResult> Edit(Guid id, Image imageModel)
        {
            if (id != imageModel.Id)
            {
                return NotFound();
            }

            Image? result;
            
            if (imageModel.ImageFile != null)
            {
                result = ValidateImage(imageModel);
            }
            else
            {
                result = imageModel;
            }

            if (result != null && ModelState.IsValid)
            {
                await _bll.Images.UpdateAsync(result, _hostEnvironment.WebRootPath);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(imageModel);
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var image = await _bll.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var image = await _bll.Images.FindAsync(id);
            _bll.Images.Remove(image, _hostEnvironment.WebRootPath);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private Image? ValidateImage(Image imageModel)
        {
            var extension = Path.GetExtension(imageModel.ImageFile.FileName);

            if (!(extension == ".png" || extension == ".jpg" || extension == ".jpeg"))
            {
                ModelState.AddModelError(string.Empty, "Extension supported only: [.png, .jpg, .jpeg]");
                return null;
            }

            using (var image = System.Drawing.Image.FromStream(imageModel.ImageFile.OpenReadStream()))
            {
                if (image.Height > 4000 || image.Width > 4000)
                {
                    ModelState.AddModelError(string.Empty, "Image should be not bigger that 4000x4000");
                    return null;
                }

                var ratio = image.Height * 1.0 / image.Width;
                if (ratio < 0.1 || 10 < ratio)
                {
                    ModelState.AddModelError(string.Empty, "Image ration should be between 0.1 and 10");
                    return null;
                }

                imageModel.HeightPx = image.Height;
                imageModel.WidthPx = image.Width;
            }

            return imageModel;
        }
    }
}