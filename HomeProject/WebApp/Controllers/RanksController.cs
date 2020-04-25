using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using Extension;
using Rank = DAL.App.DTO.Rank;

namespace WebApp.Controllers
{
    public class RanksController : Controller
    {
        private readonly IAppBLL _bll;

        public RanksController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Ranks
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Ranks.AllAsync());
        }

        // GET: Ranks/Details/5
        public async Task<IActionResult> Details(Guid id)
        {

            var rank = await _bll.Ranks.FindAsync(id);

            if (rank == null)
            {
                return NotFound();
            }

            return View(rank);
        }

        // GET: Ranks/Create
        public IActionResult Create()
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: Ranks/Create
        // To protect from overranking attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Ranks/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {


            var rank = await _bll.Ranks.FindAsync(id);

            if (rank == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", rank.ProfileId);
            return View(rank);
        }

        // POST: Ranks/Edit/5
        // To protect from overranking attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Ranks/Delete/5
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
            await _bll.Ranks.RemoveAsync(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
