using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Comment = BLL.App.DTO.Comment;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// comments
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CommentsController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public CommentsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Comments.AllAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id, string? returnUrl)
        {
            var comment = await _bll.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            comment.ReturnUrl = returnUrl;

            return View(comment);
        }

        /// <summary>
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create(Guid postId, string? returnUrl)
        {
            var comment = new Comment
            {
                PostId = postId,
                ReturnUrl = returnUrl
            };


            return View(comment);
        }

        
        /// <summary>
        /// Creates a new record
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            ModelState.Clear();
            comment.ProfileId = User.UserId();

            if (TryValidateModel(comment))
            {
                comment.Id = Guid.NewGuid();
                _bll.Comments.Add(comment);
                await _bll.SaveChangesAsync();

                await _bll.Ranks.IncreaseUserExperience(User.UserId(), 2);

                if (comment.ReturnUrl != null)
                {
                    return Redirect(comment.ReturnUrl);
                }

                return RedirectToAction(nameof(Index), "Home");
            }

            return View(comment);
        }
        
        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id, string? returnUrl)
        {
            var comment = await _bll.Comments.FindAsync(id);

            comment.ReturnUrl = returnUrl;

            return View(comment);
        }
        
        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            Comment comment)
        {
            var record = await _bll.Comments.FindAsync(id);

            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.Comments.UpdateAsync(comment);
                await _bll.SaveChangesAsync();


                if (comment.ReturnUrl != null)
                {
                    return Redirect(comment.ReturnUrl);
                }

                return RedirectToAction(nameof(Index), "Home");
            }

            return View(comment);
        }
        
        /// <summary>
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id, string? returnUrl)
        {
            var comment = await _bll.Comments.FindAsync(id);

            comment.ReturnUrl = returnUrl;

            return View(comment);
        }
        
        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Comment comment)
        {
            var record = await _bll.Comments.FindAsync(id);

            _bll.Comments.Remove(id);
            await _bll.SaveChangesAsync();

            if (comment.ReturnUrl != null)
            {
                return Redirect(comment.ReturnUrl);
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}