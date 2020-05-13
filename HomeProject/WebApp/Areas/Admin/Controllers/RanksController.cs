using System;
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
            return View(await _bll.Ranks.AllAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {

            var rank = await _bll.Ranks.FindAsync(id);

            if (rank == null)
            {
                return NotFound();
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
        public async Task<IActionResult> Create(
            BLL.App.DTO.Rank rank)
        {
            ModelState.Clear();
            rank.ChangedAt = DateTime.Now;
            rank.CreatedAt = DateTime.Now;

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
            var rank = await _bll.Ranks.FindAsync(id);

            if (rank == null)
            {
                return NotFound();
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
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.Rank rank)
        {
            if (id != rank.Id)
            {
                return NotFound();
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
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {


            var rank = await _bll.Ranks.FindAsync(id);

            if (rank == null)
            {
                return NotFound();
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
    }
}
