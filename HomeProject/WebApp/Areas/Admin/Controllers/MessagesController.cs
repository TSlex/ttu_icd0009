using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{    
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class MessagesController : Controller
    {
        private readonly IAppBLL _bll;

        public MessagesController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Messages
//        [Route("/{chatRoomId?}")]
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Messages.AllAsync());
        }
        
        public async Task<IActionResult> Details(Guid id)
        {
            var post = await _bll.Messages.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Messages/Create
        public IActionResult Create(Guid chatRoomId)
        {
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
            BLL.App.DTO.Message message)
        {
            ModelState.Clear();
            
            message.ProfileId = User.UserId();

            if (TryValidateModel(message))
            {
                message.Id = Guid.NewGuid();
                _bll.Messages.Add(message);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {

            var message = await _bll.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }
            
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overmessageing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.Messages.UpdateAsync(message);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var message = await _bll.Messages.FindAsync(id);

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
            _bll.Messages.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
