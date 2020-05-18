using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{    
    /// <summary>
    /// Messages
    /// </summary>
    [Authorize]
    [Route("{controller}/{chatRoomId}/{action=Index}/{id?}")]
    public class MessagesController : Controller
    {
        private readonly IAppBLL _bll;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public MessagesController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create(Guid chatRoomId)
        {
            var message = new Message()
            {
                ChatRoomId = chatRoomId,
            };
            return View(message);
        }

        /// <summary>
        /// Creates a new record
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.Message message)
        {
            ModelState.Clear();
            
            message.ProfileId = User.UserId();

            if (TryValidateModel(message))
            {
                message.Id = Guid.NewGuid();
                _bll.Messages.Add(message);
                await _bll.SaveChangesAsync();

                return RedirectToAction("Details", "ChatRooms", new {id = message.ChatRoomId});
            }

            return View(message);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {

            var message = await _bll.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Message message)
        {
            if (id != message.Id || User.UserId() != message.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.Messages.UpdateAsync(message);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(message);
        }

        /// <summary>
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {

            var message = await _bll.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
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
            _bll.Messages.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
