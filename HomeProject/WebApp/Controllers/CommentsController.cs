using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Repositories;
using Domain;
using Extension;

namespace WebApp.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CommentRepo _commentRepo;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
            _commentRepo = new CommentRepo(context);
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            return View(await _commentRepo.AllAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _commentRepo.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "CommentValue,CommentDateTime,ProfileId,PostId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")]
            Comment comment)
        {
            ModelState.Clear();
            comment.ProfileId = User.UserId();
            comment.ChangedAt = DateTime.Now;
            comment.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                comment.Id = Guid.NewGuid();
                _commentRepo.Add(comment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _commentRepo.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "CommentValue,CommentDateTime,ProfileId,PostId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")]
            Comment comment)
        {
            if (id != comment.Id || User.UserId() != comment.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _commentRepo.Update(comment);
                await _commentRepo.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _commentRepo.FindAsync(id);
            
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _commentRepo.Remove(id);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
}