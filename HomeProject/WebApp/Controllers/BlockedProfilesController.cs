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
    public class BlockedProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlockedProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BlockedProfiles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BlockedProfiles.Include(b => b.BProfile).Include(b => b.Profile);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BlockedProfiles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blockedProfile = await _context.BlockedProfiles
                .Include(b => b.BProfile)
                .Include(b => b.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blockedProfile == null)
            {
                return NotFound();
            }

            return View(blockedProfile);
        }

        // GET: BlockedProfiles/Create
        public IActionResult Create()
        {
            ViewData["BProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: BlockedProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfileId,BProfileId,Reason,CreatedBy,CreatedAt,DeletedBy,DeletedAt,Id")] BlockedProfile blockedProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blockedProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BProfileId"] = new SelectList(_context.Profiles, "Id", "Id", blockedProfile.BProfileId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", blockedProfile.ProfileId);
            return View(blockedProfile);
        }

        // GET: BlockedProfiles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blockedProfile = await _context.BlockedProfiles.FindAsync(id);
            if (blockedProfile == null)
            {
                return NotFound();
            }
            ViewData["BProfileId"] = new SelectList(_context.Profiles, "Id", "Id", blockedProfile.BProfileId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", blockedProfile.ProfileId);
            return View(blockedProfile);
        }

        // POST: BlockedProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProfileId,BProfileId,Reason,CreatedBy,CreatedAt,DeletedBy,DeletedAt,Id")] BlockedProfile blockedProfile)
        {
            if (id != blockedProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blockedProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlockedProfileExists(blockedProfile.Id))
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
            ViewData["BProfileId"] = new SelectList(_context.Profiles, "Id", "Id", blockedProfile.BProfileId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", blockedProfile.ProfileId);
            return View(blockedProfile);
        }

        // GET: BlockedProfiles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blockedProfile = await _context.BlockedProfiles
                .Include(b => b.BProfile)
                .Include(b => b.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blockedProfile == null)
            {
                return NotFound();
            }

            return View(blockedProfile);
        }

        // POST: BlockedProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var blockedProfile = await _context.BlockedProfiles.FindAsync(id);
            _context.BlockedProfiles.Remove(blockedProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlockedProfileExists(string id)
        {
            return _context.BlockedProfiles.Any(e => e.Id == id);
        }
    }
}