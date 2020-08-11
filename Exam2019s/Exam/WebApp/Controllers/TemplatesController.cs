using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    /// <summary>
    /// Template
    /// </summary>
    [Authorize]
    public class TemplatesController : Controller
    {
        private readonly IAppBLL _bll;
        
        /// <inheritdoc />
        public TemplatesController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Return all templates
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Templates.AllAsync());
        }

        /// <summary>
        /// Return specific template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var template = await _bll.Templates.FindAsync(id);
            
            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }

        /// <summary>
        /// Return template create page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new template
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Template template)
        {
            if (ModelState.IsValid)
            {
                template.Id = Guid.NewGuid();

                _bll.Templates.Add(template);

                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(template);
        }

        /// <summary>
        /// Return template edit page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var template = await _bll.Templates.FindAsync(id);
            
            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }

        /// <summary>
        /// Updates a template
        /// </summary>
        /// <param name="id"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Template template)
        {
            if (id != template.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.Templates.UpdateAsync(template);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(template);
        }

        /// <summary>
        /// Return template delete page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {
            var template = await _bll.Templates.FindAsync(id);
            
            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }

        /// <summary>
        /// Deletes a template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var template = await _bll.Templates.FindAsync(id);
            
            _bll.Templates.Remove(template);
            
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}