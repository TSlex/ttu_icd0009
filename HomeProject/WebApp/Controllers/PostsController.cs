using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace WebApp.Controllers
{
    /// <summary>
    /// Posts
    /// </summary>
    [Authorize]
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
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id, string? returnUrl)
        {
            var post = await _bll.Posts.GetPostFull(id);

            if (post == null || post.DeletedAt != null)
            {
                return NotFound();
            }

            post.IsUserFavorite = post.Favorites.Select(favorite => favorite.ProfileId).Contains(User.UserId());
            
            return View(post);
        }
        
        /// <summary>
        /// Add profile to favorites
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavorite(Guid id, Post post)
        {
            var userId = User.UserId();
            
            if (id != post.Id)
            {
                return RedirectToAction(nameof(Details), post);
            }
            
            var favorite = await _bll.Favorites.FindAsync(post.Id, userId);

            if (favorite == null)
            {
                _bll.Favorites.Create(post.Id, userId);
                await _bll.SaveChangesAsync();
                
                await _bll.Ranks.IncreaseUserExperience(User.UserId(), 1);
            }
            
            post = await _bll.Posts.GetPostFull(post.Id);
            
            return RedirectToAction(nameof(Details), post);
        }
        
        /// <summary>
        /// Removes post from favorites
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromFavorite(Guid id, Post post)
        {
            var userId = User.UserId();

            if (id != post.Id)
            {
                return RedirectToAction(nameof(Details), post);
            }
            
            var favorite = await _bll.Favorites.FindAsync(post.Id, userId);
            
            if (favorite != null)
            {
                await _bll.Favorites.RemoveAsync(post.Id, userId);
                await _bll.SaveChangesAsync();
            }
            
            post = await _bll.Posts.GetPostFull(post.Id);

            return RedirectToAction(nameof(Details), post);
        }

        /// <summary>
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create(string? returnUrl)
        {
            var post = new Post()
            {
                ReturnUrl = returnUrl
            };
            
            return View(post);
        }

        /// <summary>
        /// Creates a new record
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (post.PostImage!.ImageFile == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ImageRequired);
                return View(post);
            }
            
            ModelState.Clear();
            post.ProfileId = User.UserId();

            if (TryValidateModel(post))
            {
                post.Id = Guid.NewGuid();
                
                var imageModel = post.PostImage;
                imageModel.Id = Guid.NewGuid();
                imageModel.ImageType = ImageType.Post;
                imageModel.ImageFor = post.Id;

                post.PostImageId = imageModel.Id;
                post.PostImage = null;
                
                _bll.Posts.Add(post);
                await _bll.Images.AddPostAsync(post.Id, imageModel);
                await _bll.SaveChangesAsync();

                if (post.ReturnUrl != null)
                {
                    return Redirect(post.ReturnUrl);
                }

                return RedirectToAction(nameof(Index), "Profiles", new {username = User.Identity.Name});
            }

            return View(post);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id, string? returnUrl)
        {
            var post = await _bll.Posts.FindAsync(id);

            if (!ValidateUserAccess(post))
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Edit(Guid id, Post post)
        {
            var record = await _bll.Posts.GetForUpdateAsync(id);

            if (!ValidateUserAccess(record) || id != post.Id || post.PostImageId != post.PostImage.Id)
            {
                return NotFound();
            }
            
            if (post.PostImage!.ImageFile == null && post.PostImageId == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ImageRequired);
                return View(post);
            }

            ModelState.Clear();
            post.ProfileId = User.UserId();
            post.PostPublicationDateTime = record.PostPublicationDateTime;
            
            var imageModel = post.PostImage;
            if (post.PostImageId == null)
            {
                imageModel.Id = Guid.NewGuid();
                imageModel.ImageType = ImageType.Post;
                imageModel.ImageFor = post.Id;
            }

            if (TryValidateModel(post))
            {
                if (record.PostImageId == null)
                {
                    await _bll.Images.AddPostAsync(post.Id, imageModel);
                }
                else
                {
                    var imageRecord = await _bll.Images.FindAsync((Guid) record.PostImageId!);

                    imageModel.OriginalImageUrl = imageRecord.OriginalImageUrl;
                    imageModel.ImageUrl = imageRecord.ImageUrl;
                    imageModel.ImageFor = imageRecord.ImageFor;
                    imageModel.ImageType = imageRecord.ImageType;
                    
                    await _bll.Images.UpdatePostAsync(post.Id, imageModel);
                }

                post.PostImageId = imageModel.Id;
                post.PostImage = null;
                
                await _bll.Posts.UpdateAsync(post);
                await _bll.SaveChangesAsync();

                return RedirectToAction("Details", "Posts", new {id});
            }

            return View(post);
        }

        /// <summary>
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id, string? returnUrl)
        {
            var post = await _bll.Posts.GetNoIncludes(id, null);

            if (!ValidateUserAccess(post))
            {
                return NotFound();
            }
            
            post.ReturnUrl = returnUrl;

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
            var record = await _bll.Posts.GetForUpdateAsync(id);
            
            if (!ValidateUserAccess(record))
            {
                return NotFound();
            }

            _bll.Posts.Remove(id);
            await _bll.SaveChangesAsync();
            
            if (post.ReturnUrl != null)
            {
                return Redirect(post.ReturnUrl);
            }

            return RedirectToAction(nameof(Index), "Profiles", new {username = User.Identity.Name});
        }

        private bool ValidateUserAccess(Post? record)
        {
            return record != null && record.ProfileId == User.UserId();
        }
    }
}