using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
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
            return View(await _bll.ChatRooms.AllAsync(User.UserId()));
        }

        public async Task<IActionResult> OpenOrCreate(string username)
        {
            var chatRoomId = await _bll.ChatRooms.OpenOrCreateAsync(username);

            return RedirectToAction("Index", "Messages", new {
                chatRoomId = chatRoomId
            });
        }
        
        public async Task<IActionResult> Details(Guid id)
        {
            var chatRoom = await _bll.ChatRooms.FindAsync(id);

            if (chatRoom == null)
            {
                return NotFound();
            }

            return View(chatRoom);
        }
        
        public async Task<IActionResult> Edit(Guid id, string? returnUrl)
        {
            var chatRoom = await _bll.ChatRooms.FindAsync(id);

            return View(chatRoom);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ChatRoom chatRoom)
        {
            if (id != chatRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.ChatRooms.UpdateAsync(chatRoom);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index), "Home");
            }

            return View(chatRoom);
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
            _bll.ChatRooms.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}