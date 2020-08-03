using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// User black list
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class BlockedProfilesController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public BlockedProfilesController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.BlockedProfiles.AllAdminAsync());
        }


        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var blockedProfile = await _bll.BlockedProfiles.FindAdminAsync(id);

            if (blockedProfile == null)
            {
                return NotFound();
            }

            return View(blockedProfile);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var blockedProfile = await _bll.BlockedProfiles.FindAdminAsync(id);

            if (blockedProfile == null)
            {
                return NotFound();
            }

            return View(blockedProfile);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blockedProfile"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.BlockedProfile blockedProfile)
        {
            if (id != blockedProfile.Id)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorIdMatch);
            }

            if (!await _bll.Profiles.ExistsAsync(blockedProfile.ProfileId) ||
                !await _bll.Profiles.ExistsAsync(blockedProfile.BProfileId))
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorBadData);
            }

            if (ModelState.IsValid)
            {
                await _bll.BlockedProfiles.UpdateAsync(blockedProfile);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(blockedProfile);
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
            _bll.BlockedProfiles.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}