using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Favorites
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Favorites.Include(f => f.Post).Include(f => f.Profile);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Favorites/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites
                .Include(f => f.Post)
                .Include(f => f.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // GET: Favorites/Create
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id");
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: Favorites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfileId,PostId,CreatedBy,CreatedAt,DeletedBy,DeletedAt,Id")] Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favorite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", favorite.PostId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", favorite.ProfileId);
            return View(favorite);
        }

        // GET: Favorites/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", favorite.PostId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", favorite.ProfileId);
            return View(favorite);
        }

        // POST: Favorites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProfileId,PostId,CreatedBy,CreatedAt,DeletedBy,DeletedAt,Id")] Favorite favorite)
        {
            if (id != favorite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favorite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteExists(favorite.Id))
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
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", favorite.PostId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", favorite.ProfileId);
            return View(favorite);
        }

        // GET: Favorites/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites
                .Include(f => f.Post)
                .Include(f => f.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // POST: Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var favorite = await _context.Favorites.FindAsync(id);
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteExists(string id)
        {
            return _context.Favorites.Any(e => e.Id == id);
        }
    }
}