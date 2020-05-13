using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    /// <summary>
    /// Profile ranks
    /// </summary>
    [Authorize]
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
            return View(await _bll.ProfileRanks.AllAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {


            var profileRank = await _bll.ProfileRanks.FindAsync(id);

            if (profileRank == null)
            {
                return NotFound();
            }

            return View(profileRank);
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
        /// <param name="profileRank"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.ProfileRank profileRank)
        {
            ModelState.Clear();
            profileRank.ProfileId = User.UserId();
            profileRank.ChangedAt = DateTime.Now;
            profileRank.CreatedAt = DateTime.Now;

            if (TryValidateModel(profileRank))
            {
                profileRank.Id = Guid.NewGuid();
                _bll.ProfileRanks.Add(profileRank);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(profileRank);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {


            var profileRank = await _bll.ProfileRanks.FindAsync(id);

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
            if (id != profileRank.Id || User.UserId() != profileRank.ProfileId)
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
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {


            var profileRank = await _bll.ProfileRanks.FindAsync(id);

            if (profileRank == null)
            {
                return NotFound();
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
    }
}
