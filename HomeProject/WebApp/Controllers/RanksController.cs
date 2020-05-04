using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{    
    [Authorize]
    public class RanksController : Controller
    {
        private readonly IAppBLL _bll;

        public RanksController(IAppBLL bll)
        {
            _bll = bll;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Ranks.AllAsync());
        }
        
        public async Task<IActionResult> Details(Guid id)
        {

            var rank = await _bll.Ranks.FindAsync(id);

            if (rank == null)
            {
                return NotFound();
            }

            return View(rank);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
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
        
        public async Task<IActionResult> Edit(Guid id)
        {
            var rank = await _bll.Ranks.FindAsync(id);

            if (rank == null)
            {
                return NotFound();
            }
            
            return View(rank);
        }
        
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
        
        public async Task<IActionResult> Delete(Guid id)
        {
            var rank = await _bll.Ranks.FindAsync(id);

            if (rank == null)
            {
                return NotFound();
            }

            return View(rank);
        }

        // POST: Ranks/Delete/5
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
