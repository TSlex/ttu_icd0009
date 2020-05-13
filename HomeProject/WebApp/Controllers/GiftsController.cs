using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    /// <summary>
    /// Gifts
    /// </summary>
    [Authorize]
    public class GiftsController : Controller
    {
        private readonly IAppBLL _bll;
        
        /// <summary>
        /// Controller
        /// </summary>
        /// <param name="bll"></param>
        public GiftsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Gifts.AllAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {

            var gift = await _bll.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
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
        /// <param name="gift"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gift gift)
        {
            ModelState.Clear();

            if (TryValidateModel(gift))
            {
                gift.Id = Guid.NewGuid();
                _bll.Gifts.Add(gift);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(gift);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {

            var gift = await _bll.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }
            
            return View(gift);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gift"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Gift gift)
        {
            if (id != gift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.Gifts.UpdateAsync(gift);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(gift);
        }

        /// <summary>
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {

            var gift = await _bll.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
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
            _bll.Gifts.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
