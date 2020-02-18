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
    public class ProfileGiftsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileGiftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProfileGifts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProfileGifts.Include(p => p.Gift).Include(p => p.Profile);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProfileGifts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileGift = await _context.ProfileGifts
                .Include(p => p.Gift)
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.ProfileGiftId == id);
            if (profileGift == null)
            {
                return NotFound();
            }

            return View(profileGift);
        }

        // GET: ProfileGifts/Create
        public IActionResult Create()
        {
            ViewData["GiftId"] = new SelectList(_context.Gifts, "GiftId", "GiftId");
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: ProfileGifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfileGiftId,ProfileId,GiftId")] ProfileGift profileGift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profileGift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GiftId"] = new SelectList(_context.Gifts, "GiftId", "GiftId", profileGift.GiftId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profileGift.ProfileId);
            return View(profileGift);
        }

        // GET: ProfileGifts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileGift = await _context.ProfileGifts.FindAsync(id);
            if (profileGift == null)
            {
                return NotFound();
            }
            ViewData["GiftId"] = new SelectList(_context.Gifts, "GiftId", "GiftId", profileGift.GiftId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profileGift.ProfileId);
            return View(profileGift);
        }

        // POST: ProfileGifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProfileGiftId,ProfileId,GiftId")] ProfileGift profileGift)
        {
            if (id != profileGift.ProfileGiftId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profileGift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileGiftExists(profileGift.ProfileGiftId))
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
            ViewData["GiftId"] = new SelectList(_context.Gifts, "GiftId", "GiftId", profileGift.GiftId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profileGift.ProfileId);
            return View(profileGift);
        }

        // GET: ProfileGifts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileGift = await _context.ProfileGifts
                .Include(p => p.Gift)
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.ProfileGiftId == id);
            if (profileGift == null)
            {
                return NotFound();
            }

            return View(profileGift);
        }

        // POST: ProfileGifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var profileGift = await _context.ProfileGifts.FindAsync(id);
            _context.ProfileGifts.Remove(profileGift);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileGiftExists(string id)
        {
            return _context.ProfileGifts.Any(e => e.ProfileGiftId == id);
        }
    }
}
