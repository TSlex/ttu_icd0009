using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Controllers
{
    public class ChatRolesController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public ChatRolesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: ChatRoles
        public async Task<IActionResult> Index()
        {
            return View(await _uow.ChatRoles.AllAsync());
        }

        // GET: ChatRoles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRole = await _uow.ChatRoles.FindAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }

            return View(chatRole);
        }

        // GET: ChatRoles/Create
        public IActionResult Create()
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: ChatRoles/Create
        // To protect from overchatRoleing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ChatRole chatRole)
        {
            ModelState.Clear();
            chatRole.ChangedAt = DateTime.Now;
            chatRole.CreatedAt = DateTime.Now;

            if (TryValidateModel(chatRole))
            {
                chatRole.Id = Guid.NewGuid();
                _uow.ChatRoles.Add(chatRole);
                await _uow.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(chatRole);
        }

        // GET: ChatRoles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRole = await _uow.ChatRoles.FindAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", chatRole.ProfileId);
            return View(chatRole);
        }

        // POST: ChatRoles/Edit/5
        // To protect from overchatRoleing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            ChatRole chatRole)
        {
            if (id != chatRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _uow.ChatRoles.Update(chatRole);
                await _uow.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(chatRole);
        }

        // GET: ChatRoles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRole = await _uow.ChatRoles.FindAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }

            return View(chatRole);
        }

        // POST: ChatRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _uow.ChatRoles.Remove(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
