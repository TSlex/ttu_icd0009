using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class GiftsController : Controller
    {
        private readonly IAppBLL _bll;

        public GiftsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Gifts
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Gifts.AllAsync());
        }

        // GET: Gifts/Details/5
        public async Task<IActionResult> Details(Guid id)
        {

            var gift = await _bll.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
        }

        // GET: Gifts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gifts/Create
        // To protect from overgifting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Gifts/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {

            var gift = await _bll.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }
            
            return View(gift);
        }

        // POST: Gifts/Edit/5
        // To protect from overgifting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Gifts/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var gift = await _bll.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
        }

        // POST: Gifts/Delete/5
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
