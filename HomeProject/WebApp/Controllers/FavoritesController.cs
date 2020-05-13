using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IAppBLL _bll;

        public FavoritesController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Favorites.AllAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var favorite = await _bll.Favorites.FindAsync(id);

            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        /// <summary>
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Favorites/Create
        // To protect from overfavoriteing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.Favorite favorite)
        {
            ModelState.Clear();
            favorite.ProfileId = User.UserId();
            favorite.ChangedAt = DateTime.Now;
            favorite.CreatedAt = DateTime.Now;

            if (TryValidateModel(favorite))
            {
                favorite.Id = Guid.NewGuid();
                _bll.Favorites.Add(favorite);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(favorite);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {


            var favorite = await _bll.Favorites.FindAsync(id);

            if (favorite == null)
            {
                return NotFound();
            }
            
            return View(favorite);
        }

        // POST: Favorites/Edit/5
        // To protect from overfavoriteing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.Favorite favorite)
        {
            if (id != favorite.Id || User.UserId() != favorite.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.Favorites.UpdateAsync(favorite);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(favorite);
        }

        /// <summary>
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {


            var favorite = await _bll.Favorites.FindAsync(id);

            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
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
            _bll.Favorites.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}