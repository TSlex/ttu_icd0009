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
            var applicationDbContext = _context.ChatMembers.Include(c => c.ChatRole).Include(c => c.ChatRoom).Include(c => c.Profile);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ChatMembers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatMember = await _context.ChatMembers
                .Include(c => c.ChatRole)
                .Include(c => c.ChatRoom)
                .Include(c => c.Profile)
                .FirstOrDefaultAsync(m => m.ChatMemberId == id);
            if (chatMember == null)
            {
                return NotFound();
            }

            return View(chatMember);
        }

        // GET: ChatMembers/Create
        public IActionResult Create()
        {
            ViewData["ChatRoleId"] = new SelectList(_context.ChatRoles, "ChatRoleId", "ChatRoleId");
            ViewData["ChatRoomId"] = new SelectList(_context.Rooms, "ChatRoomId", "ChatRoomId");
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: ChatMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChatMemberId,ProfileId,ChatRoleId,ChatRoomId")] ChatMember chatMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChatRoleId"] = new SelectList(_context.ChatRoles, "ChatRoleId", "ChatRoleId", chatMember.ChatRoleId);
            ViewData["ChatRoomId"] = new SelectList(_context.Rooms, "ChatRoomId", "ChatRoomId", chatMember.ChatRoomId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", chatMember.ProfileId);
            return View(chatMember);
        }

        // GET: ChatMembers/Edit/5
        public async Task<IActionResult> Edit(string id)
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
            ViewData["ChatRoleId"] = new SelectList(_context.ChatRoles, "ChatRoleId", "ChatRoleId", chatMember.ChatRoleId);
            ViewData["ChatRoomId"] = new SelectList(_context.Rooms, "ChatRoomId", "ChatRoomId", chatMember.ChatRoomId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", chatMember.ProfileId);
            return View(chatMember);
        }

        // POST: ChatMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ChatMemberId,ProfileId,ChatRoleId,ChatRoomId")] ChatMember chatMember)
        {
            if (id != chatMember.ChatMemberId)
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
                    if (!ChatMemberExists(chatMember.ChatMemberId))
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
            ViewData["ChatRoleId"] = new SelectList(_context.ChatRoles, "ChatRoleId", "ChatRoleId", chatMember.ChatRoleId);
            ViewData["ChatRoomId"] = new SelectList(_context.Rooms, "ChatRoomId", "ChatRoomId", chatMember.ChatRoomId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", chatMember.ProfileId);
            return View(chatMember);
        }

        // GET: ChatMembers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatMember = await _context.ChatMembers
                .Include(c => c.ChatRole)
                .Include(c => c.ChatRoom)
                .Include(c => c.Profile)
                .FirstOrDefaultAsync(m => m.ChatMemberId == id);
            if (chatMember == null)
            {
                return NotFound();
            }

            return View(chatMember);
        }

        // POST: ChatMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var chatMember = await _context.ChatMembers.FindAsync(id);
            _context.ChatMembers.Remove(chatMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatMemberExists(string id)
        {
            return _context.ChatMembers.Any(e => e.ChatMemberId == id);
        }
    }
}
