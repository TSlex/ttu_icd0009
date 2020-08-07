using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Ranks
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class RanksController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Controllers
        /// </summary>
        /// <param name="bll"></param>
        public RanksController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Ranks.AllAdminAsync());
        }

        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.Ranks.GetRecordHistoryAsync(id)).ToList()
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
            var rank = await _bll.Ranks.FindAdminAsync(id);

            if (rank == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(rank);
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
        /// <param name="rank"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BLL.App.DTO.Rank rank)
        {
            if (rank.PreviousRankId != null && (!await _bll.Ranks.ExistsAsync((Guid) rank.PreviousRankId) ||
                                                await _bll.Ranks.PreviousRankExists((Guid) rank.PreviousRankId,
                                                    null)) ||
                rank.NextRankId != null && (!await _bll.Ranks.ExistsAsync((Guid) rank.NextRankId) ||
                                            await _bll.Ranks.NextRankExists((Guid) rank.NextRankId, null)))
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorBadData);
            }

            if (TryValidateModel(rank))
            {
                rank.Id = Guid.NewGuid();
                _bll.Ranks.Add(rank);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(rank);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var rank = await _bll.Ranks.FindAdminAsync(id);

            if (rank == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(rank);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BLL.App.DTO.Rank rank)
        {
            if (id != rank.Id)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorIdMatch);
            }

            if (rank.PreviousRankId != null &&
                (!await _bll.Ranks.ExistsAsync((Guid) rank.PreviousRankId) ||
                 await _bll.Ranks.PreviousRankExists((Guid) rank.PreviousRankId,
                     id)) ||
                rank.NextRankId != null &&
                (!await _bll.Ranks.ExistsAsync((Guid) rank.NextRankId) ||
                 await _bll.Ranks.NextRankExists((Guid) rank.NextRankId, id)) ||
                rank.PreviousRankId == id || rank.NextRankId == id
            )
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorBadData);
            }

            if (ModelState.IsValid)
            {
                await _bll.Ranks.UpdateAsync(rank);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(rank);
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
            _bll.Ranks.Remove(id);
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
            var record = await _bll.Ranks.GetForUpdateAsync(id);
            _bll.Ranks.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}