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
using ChatRoom = DAL.App.DTO.ChatRoom;

namespace WebApp.Controllers
{
    public class ChatRoomsController : Controller
    {
        private readonly IAppBLL _bll;

        public ChatRoomsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: ChatRooms
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ChatRooms.AllAsync());
        }

        public async Task<IActionResult> OpenOrCreate(string username)
        {
            return View("Index", await _bll.ChatRooms.AllAsync());
        }

        // GET: ChatRooms/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var chatRoom = await _bll.ChatRooms.FindAsync(id);

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
            await _bll.ChatRooms.RemoveAsync(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}