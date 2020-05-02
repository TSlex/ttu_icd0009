using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly IAppBLL _bll;

        public PostsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Posts.AllAsync());
        }

        // GET: Posts/Details/5
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

        // GET: Posts/Create
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

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(Guid id, string? returnUrl)
        {
            var post = await _bll.Posts.FindAsync(id);

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
            var record = await _bll.Posts.FindAsync(id);

            if (id != post.Id)
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

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(Guid id, string? returnUrl)
        {
            var post = await _bll.Posts.FindAsync(id);

            post.ReturnUrl = returnUrl;

            return View(post);
        }

        // POST: Posts/Delete/5
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

            return RedirectToAction(nameof(Index), "Profiles", new {username = User.Identity.Name});
        }
    }
}