using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Repositories;
using Domain;
using Extension;
using ChatRoom = DAL.App.DTO.ChatRoom;

namespace WebApp.Controllers
{
    public class ChatRoomsController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public ChatRoomsController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: ChatRooms
        public async Task<IActionResult> Index()
        {
            return View(await _uow.ChatRooms.AllAsync());
        }

        // GET: ChatRooms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRoom = await _uow.ChatRooms.FindAsync(id);

            if (chatRoom == null)
            {
                return NotFound();
            }

            return View(chatRoom);
        }

        // GET: ChatRooms/Create
        public IActionResult Create()
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: ChatRooms/Create
        // To protect from overchatRooming attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            DAL.App.DTO.ChatRoom chatRoom)
        {
            ModelState.Clear();
            chatRoom.ChangedAt = DateTime.Now;
            chatRoom.CreatedAt = DateTime.Now;

            if (TryValidateModel(chatRoom))
            {
                chatRoom.Id = Guid.NewGuid();
                _uow.ChatRooms.Add(chatRoom);
                await _uow.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(chatRoom);
        }

        // GET: ChatRooms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRoom = await _uow.ChatRooms.FindAsync(id);

            if (chatRoom == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", chatRoom.ProfileId);
            return View(chatRoom);
        }

        // POST: ChatRooms/Edit/5
        // To protect from overchatRooming attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            ChatRoom chatRoom)
        {
            if (id != chatRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _uow.ChatRooms.Update(chatRoom);
                await _uow.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(chatRoom);
        }

        // GET: ChatRooms/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRoom = await _uow.ChatRooms.FindAsync(id);

            if (chatRoom == null)
            {
                return NotFound();
            }

            return View(chatRoom);
        }

        // POST: ChatRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _uow.ChatRooms.Remove(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
