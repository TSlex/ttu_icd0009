using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using DAL.App.EF;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentSubject = BLL.App.DTO.StudentSubject;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class StudentSubjectsController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bll"></param>
        public StudentSubjectsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.StudentSubjects.AllAdminAsync());
        }

        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.StudentSubjects.GetRecordHistoryAsync(id)).ToList();

            return View(nameof(Index), history);
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var subject = await _bll.StudentSubjects.FindAdminAsync(id);

            if (subject == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(subject);
        }
        
        /// <summary>
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Creates a new record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentSubject model)
        {
            if (TryValidateModel(model))
            {
                model.Id = Guid.NewGuid();
                _bll.StudentSubjects.Add(model);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var subject = await _bll.StudentSubjects.FindAdminAsync(id);

            return View(subject);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentSubject model)
        {
            if (id != model.Id)
            {
                ModelState.AddModelError(string.Empty, Resources.Domain.Common.ErrorIdMatch);
            }

            if (ModelState.IsValid)
            {
                await _bll.StudentSubjects.UpdateAsync(model);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _bll.StudentSubjects.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Restores a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.StudentSubjects.GetForUpdateAsync(id);
            _bll.StudentSubjects.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}