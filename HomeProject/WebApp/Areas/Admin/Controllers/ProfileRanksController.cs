using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Profile ranks
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class ProfileRanksController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public ProfileRanksController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ProfileRanks.AllAdminAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var profileRank = await _bll.ProfileRanks.FindAdminAsync(id);

            if (profileRank == null)
            {
                return NotFound();
            }

            return View(profileRank);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {


            var profileRank = await _bll.ProfileRanks.FindAdminAsync(id);

            if (profileRank == null)
            {
                return NotFound();
            }
            
            return View(profileRank);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="profileRank"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.ProfileRank profileRank)
        {
            if (id != profileRank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.ProfileRanks.UpdateAsync(profileRank);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(profileRank);
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
            _bll.ProfileRanks.Remove(id);
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
            var record = await _bll.ProfileRanks.GetForUpdateAsync(id);
            _bll.ProfileRanks.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
