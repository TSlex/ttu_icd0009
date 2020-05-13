using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IAppBLL _bll;

        public PostsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Posts.AllAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id, string? returnUrl)
        {
            var post = await _bll.Posts.GetPostFull(id);

            if (post == null)
            {
                return NotFound();
            }
            
            var favorite = await _bll.Favorites.FindAsync(id, User.UserId());

            post.IsUserFavorite = favorite != null;
            
            post.ReturnUrl = returnUrl;

            return View(post);
        }

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

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            ModelState.Clear();
            post.ProfileId = User.UserId();

            if (TryValidateModel(post))
            {
                post.Id = Guid.NewGuid();
                _bll.Posts.Add(post);
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
            var post = await _bll.Posts.GetForUpdateAsync(id);

            if (!ValidateUserAccess(post))
            {
                return NotFound();
            }

            post.ReturnUrl = returnUrl;
            
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Post post)
        {
            var record = await _bll.Posts.GetForUpdateAsync(id);

            if (!ValidateUserAccess(record) || id != post.Id)
            {
                return NotFound();
            }

            ModelState.Clear();
            post.ProfileId = User.UserId();

            if (TryValidateModel(post))
            {
                await _bll.Posts.UpdateAsync(post);
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
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id, string? returnUrl)
        {
            var post = await _bll.Posts.GetForUpdateAsync(id);

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