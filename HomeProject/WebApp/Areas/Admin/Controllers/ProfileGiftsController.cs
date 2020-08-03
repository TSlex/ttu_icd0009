using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Profile gifts
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class ProfileGiftsController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public ProfileGiftsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ProfileGifts.AllAdminAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var profileGift = await _bll.ProfileGifts.FindAdminAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }

            return View(profileGift);
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
        /// Create a new record
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(ProfileGift profileGift)
        {
            if (!await _bll.Profiles.ExistsAsync(profileGift.ProfileId) ||
                profileGift.FromProfileId != null &&
                !await _bll.Profiles.ExistsAsync((Guid) profileGift.FromProfileId) ||
                !await _bll.Gifts.ExistsAsync(profileGift.GiftId))
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorBadData);
            }

            if (TryValidateModel(profileGift))
            {
                profileGift.Id = Guid.NewGuid();
                _bll.ProfileGifts.Add(profileGift);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(profileGift);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var profileGift = await _bll.ProfileGifts.FindAdminAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }


            return View(profileGift);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="profileGift"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProfileGift profileGift)
        {
            if (id != profileGift.Id)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorIdMatch);
            }

            if (!await _bll.Profiles.ExistsAsync(profileGift.ProfileId) ||
                profileGift.FromProfileId != null &&
                !await _bll.Profiles.ExistsAsync((Guid) profileGift.FromProfileId) ||
                !await _bll.Gifts.ExistsAsync(profileGift.GiftId))
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorBadData);
            }

            if (ModelState.IsValid)
            {
                await _bll.ProfileGifts.UpdateAsync(profileGift);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(profileGift);
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
            _bll.ProfileGifts.Remove(id);
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
            var record = await _bll.ProfileGifts.GetForUpdateAsync(id);
            _bll.ProfileGifts.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}