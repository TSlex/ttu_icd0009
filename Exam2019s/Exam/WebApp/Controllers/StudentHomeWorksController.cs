using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;

namespace WebApp.Controllers
{
    public class StudentHomeWorksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentHomeWorksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StudentHomeWorks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StudentHomeWorks.Include(s => s.HomeWork).Include(s => s.StudentSubject);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StudentHomeWorks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentHomeWork = await _context.StudentHomeWorks
                .Include(s => s.HomeWork)
                .Include(s => s.StudentSubject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentHomeWork == null)
            {
                return NotFound();
            }

            return View(studentHomeWork);
        }

        // GET: StudentHomeWorks/Create
        public IActionResult Create()
        {
            ViewData["HomeWorkId"] = new SelectList(_context.HomeWorks, "Id", "Description");
            ViewData["StudentSubjectId"] = new SelectList(_context.StudentSubjects, "Id", "Id");
            return View();
        }

        // POST: StudentHomeWorks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomeWorkId,StudentSubjectId,Grade,StudentAnswer,AnswerDateTime,IsChecked,IsAccepted,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] StudentHomeWork studentHomeWork)
        {
            if (ModelState.IsValid)
            {
                studentHomeWork.Id = Guid.NewGuid();
                _context.Add(studentHomeWork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HomeWorkId"] = new SelectList(_context.HomeWorks, "Id", "Description", studentHomeWork.HomeWorkId);
            ViewData["StudentSubjectId"] = new SelectList(_context.StudentSubjects, "Id", "Id", studentHomeWork.StudentSubjectId);
            return View(studentHomeWork);
        }

        // GET: StudentHomeWorks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentHomeWork = await _context.StudentHomeWorks.FindAsync(id);
            if (studentHomeWork == null)
            {
                return NotFound();
            }
            ViewData["HomeWorkId"] = new SelectList(_context.HomeWorks, "Id", "Description", studentHomeWork.HomeWorkId);
            ViewData["StudentSubjectId"] = new SelectList(_context.StudentSubjects, "Id", "Id", studentHomeWork.StudentSubjectId);
            return View(studentHomeWork);
        }

        // POST: StudentHomeWorks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("HomeWorkId,StudentSubjectId,Grade,StudentAnswer,AnswerDateTime,IsChecked,IsAccepted,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,DeletedBy,DeletedAt")] StudentHomeWork studentHomeWork)
        {
            if (id != studentHomeWork.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentHomeWork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentHomeWorkExists(studentHomeWork.Id))
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
            ViewData["HomeWorkId"] = new SelectList(_context.HomeWorks, "Id", "Description", studentHomeWork.HomeWorkId);
            ViewData["StudentSubjectId"] = new SelectList(_context.StudentSubjects, "Id", "Id", studentHomeWork.StudentSubjectId);
            return View(studentHomeWork);
        }

        // GET: StudentHomeWorks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentHomeWork = await _context.StudentHomeWorks
                .Include(s => s.HomeWork)
                .Include(s => s.StudentSubject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentHomeWork == null)
            {
                return NotFound();
            }

            return View(studentHomeWork);
        }

        // POST: StudentHomeWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var studentHomeWork = await _context.StudentHomeWorks.FindAsync(id);
            _context.StudentHomeWorks.Remove(studentHomeWork);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentHomeWorkExists(Guid id)
        {
            return _context.StudentHomeWorks.Any(e => e.Id == id);
        }
    }
}
