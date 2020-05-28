using System;
using System.Linq;
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
            return View(await _bll.Comments.AllAdminAsync());
        }
        
        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.Comments.GetRecordHistoryAsync(id)).ToList()
                .OrderByDescending(record => record.CreatedAt);
            
            return View(nameof(Index), history);
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
            if (TryValidateModel(comment))
            {
                comment.Id = Guid.NewGuid();
                _bll.Comments.Add(comment);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
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
            var record = await _bll.Comments.GetForUpdateAsync(id);

            if (record == null || id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.Comments.UpdateAsync(comment);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

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
            var record = await _bll.Comments.GetForUpdateAsync(id);
            _bll.Comments.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}