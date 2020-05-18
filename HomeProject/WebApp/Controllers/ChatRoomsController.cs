using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    /// <summary>
    /// chat rooms
    /// </summary>
    [Authorize]
    [Route("/{controller}/{action=Index}/{id?}")]
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
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/{controller}/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            return View(await _bll.ChatRooms.FindAsync(id));
        }

        /// <summary>
        /// Opens chat room with user (if not exist - creates)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<IActionResult> OpenOrCreate(string username)
        {
            var chatRoomId = await _bll.ChatRooms.OpenOrCreateAsync(username);

            return RedirectToAction("Details", new
            {
                Id = chatRoomId
            });
        }

        public async Task<IActionResult> LeaveChat(Guid chatRoomId)
        {
            var chatMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), chatRoomId);

            if (chatMember == null)
            {
                return NotFound();
            }

            _bll.ChatMembers.Remove(chatMember);
            await _bll.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var chatRoom = await _bll.ChatRooms.FindAsync(id);

            if (chatRoom == null)
            {
                return NotFound();
            }

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
            var record = await _bll.ChatRooms.GetForUpdateAsync(id);

            if (record == null || id != chatRoom.Id ||
                !await _bll.ChatRooms.IsRoomMemberAsync(chatRoom.Id, User.UserId()))
            {
                return NotFound();
            }
            

            if (ModelState.IsValid)
            {
                record.ChatRoomTitle = chatRoom.ChatRoomTitle;
                
                await _bll.ChatRooms.UpdateAsync(record);

                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(record);
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