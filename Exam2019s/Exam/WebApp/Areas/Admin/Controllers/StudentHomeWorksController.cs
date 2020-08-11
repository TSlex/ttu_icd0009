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
using StudentHomeWork = BLL.App.DTO.StudentHomeWork;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class StudentHomeWorksController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bll"></param>
        public StudentHomeWorksController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.StudentHomeWorks.AllAdminAsync());
        }

        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.StudentHomeWorks.GetRecordHistoryAsync(id)).ToList();

            return View(nameof(Index), history);
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var homework = await _bll.StudentHomeWorks.FindAdminAsync(id);

            if (homework == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(homework);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var homework = await _bll.StudentHomeWorks.FindAdminAsync(id);

            return View(homework);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="homework"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentHomeWork homework)
        {
            if (id != homework.Id)
            {
                ModelState.AddModelError(string.Empty, Resources.Domain.Common.ErrorIdMatch);
            }

            if (ModelState.IsValid)
            {
                await _bll.StudentHomeWorks.UpdateAsync(homework);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(homework);
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
            _bll.StudentHomeWorks.Remove(id);
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
            var record = await _bll.StudentHomeWorks.GetForUpdateAsync(id);
            _bll.StudentHomeWorks.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
