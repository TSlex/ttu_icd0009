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

        public async Task<IActionResult> OpenOrCreate(string username)
        {
            return View("Index", await _uow.ChatRooms.AllAsync());
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
