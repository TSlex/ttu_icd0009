using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Chat roles
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class ChatRolesController : Controller
    {
        private readonly IAppBLL _bll;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public ChatRolesController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ChatRoles.AllAdminAsync());
        }
        
        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.ChatRoles.GetRecordHistoryAsync(id)).ToList()
                .OrderByDescending(record => record.CreatedAt);
            
            return View(nameof(Index), history);
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var chatRole = await _bll.ChatRoles.FindAdminAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }

            return View(chatRole);
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
        /// <param name="chatRole"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.ChatRole chatRole)
        {
            ModelState.Clear();

            if (chatRole.RoleTitleValue == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorTranslationRequired);
            }

            if (TryValidateModel(chatRole))
            {
                chatRole.Id = Guid.NewGuid();
                _bll.ChatRoles.Add(chatRole);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(chatRole);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var chatRole = await _bll.ChatRoles.FindAdminAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }
            
            return View(chatRole);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="chatRole"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.ChatRole chatRole)
        {
            if (id != chatRole.Id)
            {
                return NotFound();
            }
            
            if (chatRole.RoleTitleValue == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorTranslationRequired);
            }

            if (ModelState.IsValid)
            {
                await _bll.ChatRoles.UpdateAsync(chatRole);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(chatRole);
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
            _bll.ChatRoles.Remove(id);
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
            var record = await _bll.ChatRoles.GetForUpdateAsync(id);
            _bll.ChatRoles.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
