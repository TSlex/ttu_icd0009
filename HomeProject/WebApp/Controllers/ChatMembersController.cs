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
    public class ChatMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChatMembers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ChatMembers.Include(c => c.Profile);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ChatMembers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatMember = await _context.ChatMembers
                .Include(c => c.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatMember == null)
            {
                return NotFound();
            }

            return View(chatMember);
        }

        // GET: ChatMembers/Create
        public IActionResult Create()
        {
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: ChatMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfileId,ChatRoleId,ChatRoomId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] ChatMember chatMember)
        {
            if (ModelState.IsValid)
            {
                chatMember.Id = Guid.NewGuid();
                _context.Add(chatMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", chatMember.ProfileId);
            return View(chatMember);
        }

        // GET: ChatMembers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatMember = await _context.ChatMembers.FindAsync(id);
            if (chatMember == null)
            {
                return NotFound();
            }
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", chatMember.ProfileId);
            return View(chatMember);
        }

        // POST: ChatMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProfileId,ChatRoleId,ChatRoomId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] ChatMember chatMember)
        {
            if (id != chatMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatMemberExists(chatMember.Id))
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
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", chatMember.ProfileId);
            return View(chatMember);
        }

        // GET: ChatMembers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatMember = await _context.ChatMembers
                .Include(c => c.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatMember == null)
            {
                return NotFound();
            }

            return View(chatMember);
        }

        // POST: ChatMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var chatMember = await _context.ChatMembers.FindAsync(id);
            _context.ChatMembers.Remove(chatMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatMemberExists(Guid id)
        {
            return _context.ChatMembers.Any(e => e.Id == id);
        }
    }
}
