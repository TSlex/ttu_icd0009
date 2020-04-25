using System;
using System.Threading.Tasks;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Message = DAL.App.DTO.Message;

namespace WebApp.Controllers
{    
    [Authorize]
    [Route("{controller}/{chatRoomId}/{action=Index}/{id?}")]
    public class MessagesController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public MessagesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Messages
//        [Route("/{chatRoomId?}")]
        public async Task<IActionResult> Index(Guid? chatRoomId)
        {
            if (chatRoomId == null)
            {
                return NotFound();
            }
            return View(await _uow.Messages.AllAsync());
        }

        // GET: Messages/Create
        public IActionResult Create(Guid chatRoomId)
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            var message = new Message()
            {
                ChatRoomId = chatRoomId,
            };
            return View(message);
        }

        // POST: Messages/Create
        // To protect from overmessageing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            DAL.App.DTO.Message message)
        {
            ModelState.Clear();
//
            message.ProfileId = User.UserId();
//            message.ChangedAt = DateTime.Now;
//            message.CreatedAt = DateTime.Now;

            if (TryValidateModel(message))
            {
                message.Id = Guid.NewGuid();
                _uow.Messages.Add(message);
                await _uow.SaveChangesAsync();

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

            var message = await _uow.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", message.ProfileId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overmessageing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            Message message)
        {
            if (id != message.Id || User.UserId() != message.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _uow.Messages.Update(message);
                await _uow.SaveChangesAsync();


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

            var message = await _uow.Messages.FindAsync(id);

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
            _uow.Messages.Remove(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
