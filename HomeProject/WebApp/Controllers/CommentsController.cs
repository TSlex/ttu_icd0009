using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Repositories;
using Domain;
using Extension;

namespace WebApp.Controllers
{
    public class CommentsController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public CommentsController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Comments.AllAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _uow.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: Comments/Create
        // To protect from overcommenting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Comment comment)
        {
            ModelState.Clear();
            comment.ProfileId = User.UserId();
            comment.ChangedAt = DateTime.Now;
            comment.CreatedAt = DateTime.Now;

            if (TryValidateModel(comment))
            {
                comment.Id = Guid.NewGuid();
                _uow.Comments.Add(comment);
                await _uow.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _uow.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", comment.ProfileId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overcommenting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            Comment comment)
        {
            if (id != comment.Id || User.UserId() != comment.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _uow.Comments.Update(comment);
                await _uow.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _uow.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _uow.Comments.Remove(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}