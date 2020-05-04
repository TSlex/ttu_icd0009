using System;
using System.IO;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImagesController(IAppBLL bll, IWebHostEnvironment hostEnvironment)
        {
            _bll = bll;
            _hostEnvironment = hostEnvironment;
            _bll.Images.RootPath = hostEnvironment.WebRootPath;
        }
        
        [Route("{controller}/{id?}")]
        public async Task<IActionResult> GetImage(Guid? id)
        {
            if (id == null)
            {
                return base.File("~/localstorage/images/misc/404.png", "image/jpeg");
            }
            
            var image = await _bll.Images.FindAsync(id?? Guid.Empty);

            if (image == null)
            {
                return base.File("~/localstorage/images/misc/404.png", "image/jpeg");
            }
            
            return base.File("~/localstorage" + image.ImageUrl, "image/jpeg");
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
            
            if (imageModel.ImageType != ImageType.Undefined && imageModel.ImageFor == null)
            {
                ModelState.AddModelError(string.Empty, "Id should be specified if not misc image type");
                return View(imageModel);
            }
            
            ModelState.Clear();
            
            var result = ValidateImage(imageModel);
            
            if (result != null && ModelState.IsValid)
            {
                result.Id = Guid.NewGuid();
                switch (result.ImageType)
                {
                    case ImageType.ProfileAvatar:
                        await _bll.Images.AddProfileAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    case ImageType.Post:
                        await _bll.Images.AddPostAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    case ImageType.Gift:
                        await _bll.Images.AddGiftAsync(result.ImageFor?? Guid.Empty, result);
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

            if (imageModel.ImageType != ImageType.Undefined && imageModel.ImageFor == null)
            {
                ModelState.AddModelError(string.Empty, "Id should be specified if not misc image type");
                return View(imageModel);
            }
            
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
                switch (result.ImageType)
                {
                    case ImageType.ProfileAvatar:
                        await _bll.Images.UpdateProfileAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    case ImageType.Post:
                        await _bll.Images.UpdatePostAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    case ImageType.Gift:
                        await _bll.Images.UpdateGiftAsync(result.ImageFor?? Guid.Empty, result);
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
            _bll.Images.Remove(image);
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