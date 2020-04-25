using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Repositories;
using Domain;
using Extension;
using ChatMember = DAL.App.DTO.ChatMember;

namespace WebApp.Controllers
{
    public class ChatMembersController : Controller
    {
        private readonly IAppBLL _bll;

        public ChatMembersController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: ChatMembers
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ChatMembers.AllAsync());
        }

        // GET: ChatMembers/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var chatMember = await _bll.ChatMembers.FindAsync(id);

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
        // To protect from overchatMembering attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.ChatMember chatMember)
        {
            ModelState.Clear();
            chatMember.ChangedAt = DateTime.Now;
            chatMember.CreatedAt = DateTime.Now;

            if (TryValidateModel(chatMember))
            {
                chatMember.Id = Guid.NewGuid();
                _bll.ChatMembers.Add(chatMember);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(chatMember);
        }

        // GET: ChatMembers/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var chatMember = await _bll.ChatMembers.FindAsync(id);

            if (chatMember == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", chatMember.ProfileId);
            return View(chatMember);
        }

        // POST: ChatMembers/Edit/5
        // To protect from overchatMembering attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.ChatMember chatMember)
        {
            if (id != chatMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.ChatMembers.UpdateAsync(chatMember);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(chatMember);
        }

        // GET: ChatMembers/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var chatMember = await _bll.ChatMembers.FindAsync(id);

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
            await _bll.ChatMembers.RemoveAsync(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
