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
    /// Messages
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
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
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Messages.AllAdminAsync());
        }
        
        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.Messages.GetRecordHistoryAsync(id)).ToList()
                ;
            
            return View(nameof(Index), history);
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var post = await _bll.Messages.FindAdminAsync(id);

            if (post == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(post);
        }

        /// <summary>
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Creates a new record
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Message message)
        {
            if (!await _bll.Profiles.ExistsAsync(message.ProfileId) ||
                !await _bll.ChatRooms.ExistsAsync(message.ChatRoomId))
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorBadData);
            }
            
            if (TryValidateModel(message))
            {
                message.Id = Guid.NewGuid();
                _bll.Messages.Add(message);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(message);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var message = await _bll.Messages.FindAdminAsync(id);

            if (message == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
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
            if (id != message.Id)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorIdMatch);
            }
            
            if (!await _bll.Profiles.ExistsAsync(message.ProfileId) ||
                !await _bll.ChatRooms.ExistsAsync(message.ChatRoomId))
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorBadData);
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
        
        /// <summary>
        /// Restores a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.Messages.GetForUpdateAsync(id);
            _bll.Messages.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}