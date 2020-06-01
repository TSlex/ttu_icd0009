using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// chat rooms
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ChatRoomsController : Controller
    {
        private readonly IAppBLL _bll;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public ChatRoomsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ChatRooms.AllAdminAsync());
        }
        
        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.ChatRooms.GetRecordHistoryAsync(id)).ToList()
                .OrderByDescending(record => record.CreatedAt);
            
            return View(nameof(Index), history);
        }

        /// <summary>
        /// Opens chat room with user (if not exist - creates)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<IActionResult> OpenOrCreate(string username)
        {
            var chatRoomId = await _bll.ChatRooms.OpenOrCreateAsync(username);

            return RedirectToAction("Index", "Messages", new {
                chatRoomId = chatRoomId
            });
        }
        
        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var chatRoom = await _bll.ChatRooms.FindAdminAsync(id);

            if (chatRoom == null)
            {
                return NotFound();
            }

            return View(chatRoom);
        }
        
        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id, string? returnUrl)
        {
            var chatRoom = await _bll.ChatRooms.FindAdminAsync(id);

            return View(chatRoom);
        }
        
        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="chatRoom"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _bll.ChatRooms.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        /// <summary>
        /// Restores a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.ChatRooms.GetForUpdateAsync(id);
            _bll.ChatRooms.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}