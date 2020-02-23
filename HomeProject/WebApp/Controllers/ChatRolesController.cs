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
    public class ChatRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChatRoles
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChatRoles.ToListAsync());
        }

        // GET: ChatRoles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRole = await _context.ChatRoles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatRole == null)
            {
                return NotFound();
            }

            return View(chatRole);
        }

        // GET: ChatRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleTitle,CreatedBy,CreatedAt,DeletedBy,DeletedAt,Id")] ChatRole chatRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatRole);
        }

        // GET: ChatRoles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRole = await _context.ChatRoles.FindAsync(id);
            if (chatRole == null)
            {
                return NotFound();
            }
            return View(chatRole);
        }

        // POST: ChatRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RoleTitle,CreatedBy,CreatedAt,DeletedBy,DeletedAt,Id")] ChatRole chatRole)
        {
            if (id != chatRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatRoleExists(chatRole.Id))
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
            return View(chatRole);
        }

        // GET: ChatRoles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRole = await _context.ChatRoles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatRole == null)
            {
                return NotFound();
            }

            return View(chatRole);
        }

        // POST: ChatRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var chatRole = await _context.ChatRoles.FindAsync(id);
            _context.ChatRoles.Remove(chatRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatRoleExists(string id)
        {
            return _context.ChatRoles.Any(e => e.Id == id);
        }
    }
}
