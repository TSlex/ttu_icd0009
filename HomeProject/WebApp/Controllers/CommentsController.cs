using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Comment = BLL.App.DTO.Comment;

namespace WebApp.Controllers
{
    /// <summary>
    /// comments
    /// </summary>
    [Authorize]
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
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create(Guid postId, string? returnUrl)
        {
            var post = _bll.Posts.GetForUpdateAsync(postId).Result;

            if (post == null)
            {
                return NotFound();
            }
            
            var comment = new Comment
            {
                PostId = postId, 
                Post = post,
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

                return RedirectToAction("Details", "Posts", new {id = comment.PostId});
            }

            return View(comment);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id, string? returnUrl)
        {
            var comment = await _bll.Comments.FindAsync(id);

            if (!ValidateUserAccess(comment))
            {
                return NotFound();
            }
            
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
            
            if (!ValidateUserAccess(record) || id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                record.CommentValue = comment.CommentValue;
                
                await _bll.Comments.UpdateAsync(record);
                await _bll.SaveChangesAsync();

                return RedirectToAction("Details", "Posts", new {id = record.PostId});
            }

            return View(record);
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
            var record = await _bll.Comments.GetForUpdateAsync(id);
            var post = await _bll.Posts.GetForUpdateAsync(comment.PostId);

            if (!ValidateUserAccess(record) && !(post != null && post.ProfileId == User.UserId()))
            {
                return NotFound();
            }
            
            _bll.Comments.Remove(record.Id);
            await _bll.SaveChangesAsync();

            if (comment.ReturnUrl != null)
            {
                return Redirect(comment.ReturnUrl);
            }

            return RedirectToAction("Details", "Posts", new {id = record.PostId});
        }
        
        private bool ValidateUserAccess(Comment? record)
        {
            return record != null && record.ProfileId == User.UserId();
        }
    }
}