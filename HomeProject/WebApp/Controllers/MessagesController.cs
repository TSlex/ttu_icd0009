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
using Extension;

namespace WebApp.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly MessageRepo _messageRepo;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
            _messageRepo = new MessageRepo(context);
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            return View(await _messageRepo.AllAsync());
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _messageRepo.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MessageValue,MessageDateTime,ProfileId,ChatRoomId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] Message message)
        {
            ModelState.Clear();
            message.ProfileId = User.UserId();
            message.ChangedAt = DateTime.Now;
            message.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                message.Id = Guid.NewGuid();
                _messageRepo.Add(message);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _messageRepo.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MessageValue,MessageDateTime,ProfileId,ChatRoomId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] Message message)
        {
            if (id != message.Id || User.UserId() != message.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _messageRepo.Update(message);
                await _messageRepo.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _messageRepo.FindAsync(id);
            
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var message = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
