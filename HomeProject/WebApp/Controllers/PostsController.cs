using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Repositories;
using Domain;
using Domain.Identity;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApp.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PostRepo _postRepo;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
            _postRepo = new PostRepo(_context);
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _postRepo.AllAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postRepo.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("PostTitle,PostImageUrl,PostDescription")]
            Post post)
        {
            ModelState.Clear();
            post.ProfileId = User.UserId();
            post.ChangedAt = DateTime.Now;
            post.CreatedAt = DateTime.Now;

            if (TryValidateModel(post))
            {
                post.Id = Guid.NewGuid();
                _postRepo.Add(post);
                await _postRepo.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postRepo.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", post.ProfileId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "PostTitle,PostImageUrl,PostDescription, Id, ProfileId")]
            Post post)
        {
            if (id != post.Id || User.UserId() != post.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var oldRecord = await _postRepo.FindAsync(id);

                oldRecord.PostTitle = post.PostTitle;
                oldRecord.PostImageUrl = post.PostImageUrl;
                oldRecord.PostDescription = post.PostDescription;
                
                _postRepo.Update(oldRecord);
                await _postRepo.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postRepo.FindAsync(id);
            
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _postRepo.Remove(id);
            await _postRepo.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
}