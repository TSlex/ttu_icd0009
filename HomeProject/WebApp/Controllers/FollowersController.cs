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
    public class FollowersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FollowersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Followers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Watchers.Include(f => f.FollowerProfile).Include(f => f.Profile);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Followers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follower = await _context.Watchers
                .Include(f => f.FollowerProfile)
                .Include(f => f.Profile)
                .FirstOrDefaultAsync(m => m.FollowerId == id);
            if (follower == null)
            {
                return NotFound();
            }

            return View(follower);
        }

        // GET: Followers/Create
        public IActionResult Create()
        {
            ViewData["FollowerProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: Followers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FollowerId,ProfileId,FollowerProfileId")] Follower follower)
        {
            if (ModelState.IsValid)
            {
                _context.Add(follower);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FollowerProfileId"] = new SelectList(_context.Profiles, "Id", "Id", follower.FollowerProfileId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", follower.ProfileId);
            return View(follower);
        }

        // GET: Followers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follower = await _context.Watchers.FindAsync(id);
            if (follower == null)
            {
                return NotFound();
            }
            ViewData["FollowerProfileId"] = new SelectList(_context.Profiles, "Id", "Id", follower.FollowerProfileId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", follower.ProfileId);
            return View(follower);
        }

        // POST: Followers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FollowerId,ProfileId,FollowerProfileId")] Follower follower)
        {
            if (id != follower.FollowerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(follower);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FollowerExists(follower.FollowerId))
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
            ViewData["FollowerProfileId"] = new SelectList(_context.Profiles, "Id", "Id", follower.FollowerProfileId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", follower.ProfileId);
            return View(follower);
        }

        // GET: Followers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follower = await _context.Watchers
                .Include(f => f.FollowerProfile)
                .Include(f => f.Profile)
                .FirstOrDefaultAsync(m => m.FollowerId == id);
            if (follower == null)
            {
                return NotFound();
            }

            return View(follower);
        }

        // POST: Followers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var follower = await _context.Watchers.FindAsync(id);
            _context.Watchers.Remove(follower);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FollowerExists(string id)
        {
            return _context.Watchers.Any(e => e.FollowerId == id);
        }
    }
}
