using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;

namespace WebApp.Controllers
{
    public class PostCategorysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostCategorysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PostCategorys
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PostCategories.Include(p => p.Category).Include(p => p.Post);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PostCategorys/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postCategory = await _context.PostCategories
                .Include(p => p.Category)
                .Include(p => p.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postCategory == null)
            {
                return NotFound();
            }

            return View(postCategory);
        }

        // GET: PostCategorys/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id");
            return View();
        }

        // POST: PostCategorys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,CategoryId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt")] PostCategory postCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", postCategory.CategoryId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", postCategory.PostId);
            return View(postCategory);
        }

        // GET: PostCategorys/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postCategory = await _context.PostCategories.FindAsync(id);
            if (postCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", postCategory.CategoryId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", postCategory.PostId);
            return View(postCategory);
        }

        // POST: PostCategorys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PostId,CategoryId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt")] PostCategory postCategory)
        {
            if (id != postCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostCategoryExists(postCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", postCategory.CategoryId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", postCategory.PostId);
            return View(postCategory);
        }

        // GET: PostCategorys/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postCategory = await _context.PostCategories
                .Include(p => p.Category)
                .Include(p => p.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postCategory == null)
            {
                return NotFound();
            }

            return View(postCategory);
        }

        // POST: PostCategorys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var postCategory = await _context.PostCategories.FindAsync(id);
            _context.PostCategories.Remove(postCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostCategoryExists(string id)
        {
            return _context.PostCategories.Any(e => e.Id == id);
        }
    }
}
