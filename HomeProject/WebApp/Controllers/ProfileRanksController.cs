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
    public class ProfileRanksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileRanksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProfileRanks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProfileRanks.Include(p => p.Profile).Include(p => p.Rank);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProfileRanks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileRank = await _context.ProfileRanks
                .Include(p => p.Profile)
                .Include(p => p.Rank)
                .FirstOrDefaultAsync(m => m.ProfileRankId == id);
            if (profileRank == null)
            {
                return NotFound();
            }

            return View(profileRank);
        }

        // GET: ProfileRanks/Create
        public IActionResult Create()
        {
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            ViewData["RankId"] = new SelectList(_context.Ranks, "RankId", "RankId");
            return View();
        }

        // POST: ProfileRanks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfileRankId,ProfileId,RankId")] ProfileRank profileRank)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profileRank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profileRank.ProfileId);
            ViewData["RankId"] = new SelectList(_context.Ranks, "RankId", "RankId", profileRank.RankId);
            return View(profileRank);
        }

        // GET: ProfileRanks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileRank = await _context.ProfileRanks.FindAsync(id);
            if (profileRank == null)
            {
                return NotFound();
            }
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profileRank.ProfileId);
            ViewData["RankId"] = new SelectList(_context.Ranks, "RankId", "RankId", profileRank.RankId);
            return View(profileRank);
        }

        // POST: ProfileRanks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProfileRankId,ProfileId,RankId")] ProfileRank profileRank)
        {
            if (id != profileRank.ProfileRankId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profileRank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileRankExists(profileRank.ProfileRankId))
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
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profileRank.ProfileId);
            ViewData["RankId"] = new SelectList(_context.Ranks, "RankId", "RankId", profileRank.RankId);
            return View(profileRank);
        }

        // GET: ProfileRanks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileRank = await _context.ProfileRanks
                .Include(p => p.Profile)
                .Include(p => p.Rank)
                .FirstOrDefaultAsync(m => m.ProfileRankId == id);
            if (profileRank == null)
            {
                return NotFound();
            }

            return View(profileRank);
        }

        // POST: ProfileRanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var profileRank = await _context.ProfileRanks.FindAsync(id);
            _context.ProfileRanks.Remove(profileRank);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileRankExists(string id)
        {
            return _context.ProfileRanks.Any(e => e.ProfileRankId == id);
        }
    }
}
