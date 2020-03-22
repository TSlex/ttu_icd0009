using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Repositories;
using Domain;
using Extension;

namespace WebApp.Controllers
{
    public class ChatMembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ChatMemberRepo _chatMemberRepo;

        public ChatMembersController(ApplicationDbContext context)
        {
            _context = context;
            _chatMemberRepo = new ChatMemberRepo(context);
        }

        // GET: ChatMembers
        public async Task<IActionResult> Index()
        {
            return View(await _chatMemberRepo.AllAsync());
        }

        // GET: ChatMembers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatMember = await _chatMemberRepo.FindAsync(id);

            if (chatMember == null)
            {
                return NotFound();
            }

            return View(chatMember);
        }

        // GET: ChatMembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ProfileId,ChatRoleId,ChatRoomId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")]
            ChatMember chatMember)
        {
            ModelState.Clear();
            chatMember.ProfileId = User.UserId();
            chatMember.ChangedAt = DateTime.Now;
            chatMember.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                chatMember.Id = Guid.NewGuid();
                _chatMemberRepo.Add(chatMember);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(chatMember);
        }

        // GET: ChatMembers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatMember = await _chatMemberRepo.FindAsync(id);

            if (chatMember == null)
            {
                return NotFound();
            }

            return View(chatMember);
        }

        // POST: ChatMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("ProfileId,ChatRoleId,ChatRoomId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")]
            ChatMember chatMember)
        {
            if (id != chatMember.Id || User.UserId() != chatMember.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _chatMemberRepo.Update(chatMember);
                await _chatMemberRepo.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(chatMember);
        }

        // GET: ChatMembers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatMember = await _chatMemberRepo.FindAsync(id);
            
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
            _chatMemberRepo.Remove(id);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
