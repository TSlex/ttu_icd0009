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
    /// Posts
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class PostsController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        /// <param name="hostEnvironment"></param>
        public PostsController(IAppBLL bll, IWebHostEnvironment hostEnvironment)
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
            return View(await _bll.Posts.AllAdminAsync());
        }
        
        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.Posts.GetRecordHistoryAsync(id)).ToList()
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
            var post = await _bll.Posts.FindAdminAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            
            return View(post);
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
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(104857600)] 
        public async Task<IActionResult> Create(Post post)
        {
            if (!await _bll.Profiles.ExistsAsync(post.ProfileId))
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorBadData);
                return View(post);
            }
            
            if (post.PostImage!.ImageFile == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ImageRequired);
                return View(post);
            }
            
            var (imageModel, errors) = _bll.Images.ValidateImage(post.PostImage);
            
            if (errors.Length > 0)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            
            if (TryValidateModel(post))
            {
                post.Id = Guid.NewGuid();
                
                imageModel.Id = Guid.NewGuid();
                imageModel.ImageType = ImageType.Post;
                imageModel.ImageFor = post.Id;
                
                post.PostImageId = imageModel.Id;
                post.PostImage = null;
                
                await _bll.Images.AddPostAsync(post.Id, imageModel);
                
                _bll.Posts.Add(post);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id, string? returnUrl)
        {
            var post = await _bll.Posts.FindAdminAsync(id);

            post.ReturnUrl = returnUrl;
            
            return View(post);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(104857600)] 
        public async Task<IActionResult> Edit(Guid id, Post post)
        {
            if (id != post.Id)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorIdMatch);
                return View(post);
            }
            
            if (!await _bll.Profiles.ExistsAsync(post.ProfileId))
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorBadData);
                return View(post);
            }
            
            if (post.PostImage!.ImageFile == null && post.PostImageId == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ImageRequired);
                return View(post);
            }

            ModelState.Clear();
            
            var imageModel = post.PostImage;
            
            if (post.PostImage.ImageFile != null)
            {
                var (_, errors) = _bll.Images.ValidateImage(post.PostImage);

                if (errors.Length > 0)
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }

            if (post.PostImageId == null)
            {
                imageModel.Id = Guid.NewGuid();
                imageModel.ImageType = ImageType.Post;
                imageModel.ImageFor = post.Id;
            }
            
            post.ProfileId = User.UserId();

            if (TryValidateModel(post))
            {
                if (post.PostImageId == null)
                {
                    await _bll.Images.AddPostAsync(post.Id, imageModel);
                }
                else
                {
                    await _bll.Images.UpdatePostAsync(post.Id, imageModel);
                }

                post.PostImageId = imageModel.Id;
                post.PostImage = null;
                
                await _bll.Posts.UpdateAsync(post);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Post post)
        {
            _bll.Posts.Remove(id);
            await _bll.SaveChangesAsync();
            
            if (post.ReturnUrl != null)
            {
                return Redirect(post.ReturnUrl);
            }

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
            var record = await _bll.Posts.GetForUpdateAsync(id);
            _bll.Posts.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}