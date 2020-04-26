using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Comment = BLL.App.DTO.Comment;

namespace WebApp.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly IAppBLL _bll;

        public CommentsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Comments.AllAsync());
        }

        // GET: Comments/Details/5
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

        // GET: Comments/Create
        public IActionResult Create(Guid postId, string? returnUrl)
        {
            var comment = new Comment
            {
                PostId = postId, 
                ReturnUrl = returnUrl
            };


            return View(comment);
        }

        // POST: Comments/Create
        // To protect from overcommenting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                
                if (comment.ReturnUrl != null)
                {
                    return Redirect(comment.ReturnUrl);
                }

                return RedirectToAction(nameof(Index), "Home");
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
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

        // POST: Comments/Edit/5
        // To protect from overcommenting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            Comment comment)
        {
            var record = await _bll.Comments.FindAsync(id);
            
            if (!ValidateUserAccess(record) || id != comment.Id)
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

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(Guid id, string? returnUrl)
        {
            var comment = await _bll.Comments.FindAsync(id);

            if (!ValidateUserAccess(comment))
            {
                return NotFound();
            }
            
            comment.ReturnUrl = returnUrl;

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Comment comment)
        {
            var record = await _bll.Comments.FindAsync(id);

            if (!ValidateUserAccess(record))
            {
                return NotFound();
            }
            
            _bll.Comments.Remove(id);
            await _bll.SaveChangesAsync();

            if (comment.ReturnUrl != null)
            {
                return Redirect(comment.ReturnUrl);
            }

            return RedirectToAction(nameof(Index), "Home");
        }
        
        private bool ValidateUserAccess(Comment? record)
        {
            return record != null && record.ProfileId == User.UserId();
        }
    }
}