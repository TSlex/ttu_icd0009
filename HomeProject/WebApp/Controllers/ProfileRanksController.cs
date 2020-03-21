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
            var applicationDbContext = _context.ProfileRanks.Include(p => p.Profile);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProfileRanks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileRank = await _context.ProfileRanks
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            return View();
        }

        // POST: ProfileRanks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfileId,RankId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] ProfileRank profileRank)
        {
            if (ModelState.IsValid)
            {
                profileRank.Id = Guid.NewGuid();
                _context.Add(profileRank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profileRank.ProfileId);
            return View(profileRank);
        }

        // GET: ProfileRanks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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
            return View(profileRank);
        }

        // POST: ProfileRanks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProfileId,RankId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] ProfileRank profileRank)
        {
            if (id != profileRank.Id)
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
                    if (!ProfileRankExists(profileRank.Id))
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
            return View(profileRank);
        }

        // GET: ProfileRanks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileRank = await _context.ProfileRanks
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profileRank == null)
            {
                return NotFound();
            }

            return View(profileRank);
        }

        // POST: ProfileRanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var profileRank = await _context.ProfileRanks.FindAsync(id);
            _context.ProfileRanks.Remove(profileRank);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileRankExists(Guid id)
        {
            return _context.ProfileRanks.Any(e => e.Id == id);
        }
    }
}
