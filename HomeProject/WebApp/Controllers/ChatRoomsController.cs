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

namespace WebApp.Controllers
{
    public class ChatRoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ChatRoomRepo _chatRoomRepo;

        public ChatRoomsController(ApplicationDbContext context)
        {
            _context = context;
            _chatRoomRepo = new ChatRoomRepo(context);
        }

        // GET: ChatRooms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.ToListAsync());
        }

        // GET: ChatRooms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRoom = await _chatRoomRepo.FindAsync(id);
            
            if (chatRoom == null)
            {
                return NotFound();
            }

            return View(chatRoom);
        }

        // GET: ChatRooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatRooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChatRoomTitle,LastMessageValue,LastMessageDateTime,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] ChatRoom chatRoom)
        {
            ModelState.Clear();
            chatRoom.ChangedAt = DateTime.Now;
            chatRoom.CreatedAt = DateTime.Now;
            
            if (ModelState.IsValid)
            {
                chatRoom.Id = Guid.NewGuid();
                _chatRoomRepo.Add(chatRoom);
                await _chatRoomRepo.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(chatRoom);
        }

        // GET: ChatRooms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRoom = await _chatRoomRepo.FindAsync(id);
            
            if (chatRoom == null)
            {
                return NotFound();
            }
            
            return View(chatRoom);
        }

        // POST: ChatRooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ChatRoomTitle,LastMessageValue,LastMessageDateTime,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] ChatRoom chatRoom)
        {
            if (id != chatRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _chatRoomRepo.Update(chatRoom);
                await _chatRoomRepo.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(chatRoom);
        }

        // GET: ChatRooms/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var charRoom = await _chatRoomRepo.FindAsync(id);
            
            if (charRoom == null)
            {
                return NotFound();
            }

            return View(charRoom);
        }

        // POST: ChatRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _chatRoomRepo.Remove(id);
            await _chatRoomRepo.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
