using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.EF;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers
{
    public class HomeWorksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeWorksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HomeWorks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HomeWorks.Include(h => h.Subject);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HomeWorks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeWork = await _context.HomeWorks
                .Include(h => h.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeWork == null)
            {
                return NotFound();
            }

            return View(homeWork);
        }

        // GET: HomeWorks/Create
        public IActionResult Create()
        {
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "SubjectCode");
            return View();
        }

        // POST: HomeWorks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Deadline,SubjectId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] HomeWork homeWork)
        {
            if (ModelState.IsValid)
            {
                homeWork.Id = Guid.NewGuid();
                _context.Add(homeWork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "SubjectCode", homeWork.SubjectId);
            return View(homeWork);
        }

        // GET: HomeWorks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeWork = await _context.HomeWorks.FindAsync(id);
            if (homeWork == null)
            {
                return NotFound();
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "SubjectCode", homeWork.SubjectId);
            return View(homeWork);
        }

        // POST: HomeWorks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Description,Deadline,SubjectId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] HomeWork homeWork)
        {
            if (id != homeWork.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homeWork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeWorkExists(homeWork.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "SubjectCode", homeWork.SubjectId);
            return View(homeWork);
        }

        // GET: HomeWorks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeWork = await _context.HomeWorks
                .Include(h => h.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeWork == null)
            {
                return NotFound();
            }

            return View(homeWork);
        }

        // POST: HomeWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var homeWork = await _context.HomeWorks.FindAsync(id);
            _context.HomeWorks.Remove(homeWork);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeWorkExists(Guid id)
        {
            return _context.HomeWorks.Any(e => e.Id == id);
        }
    }
}
