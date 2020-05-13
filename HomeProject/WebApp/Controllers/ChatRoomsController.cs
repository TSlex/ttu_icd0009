using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    /// <summary>
    /// chat rooms
    /// </summary>
    [Authorize]
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
            return View(await _bll.ChatRooms.AllAsync(User.UserId()));
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
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {
            var chatRoom = await _bll.ChatRooms.FindAsync(id);

            if (chatRoom == null)
            {
                return NotFound();
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
    }
}