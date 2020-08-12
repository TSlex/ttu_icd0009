using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;
using Extensions;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Student, Teacher, Admin")]
    public class StudentHomeWorksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentHomeWorksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var query = _context.HomeWorks
                .Include(hw => hw.Subject)
                .ThenInclude(hw => hw.StudentSubjects)
                .ThenInclude(sb => sb.Student)
                .Include(hw => hw.Subject)
                .ThenInclude(hw => hw.StudentSubjects)
                .ThenInclude(hw => hw.StudentHomeWorks)
                .Where(hw => hw.Subject.StudentSubjects.Select(ssb => ssb.StudentId).Contains(User.UserId()));

            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid id)
        {
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

        public async Task<IActionResult> Create(Guid homeworkId, Guid studentSubjectId)
        {
            var homework = await _context.HomeWorks
                .Include(hw => hw.Subject)
                .FirstOrDefaultAsync(hw => hw.Id == homeworkId);

            var subject = await _context.StudentSubjects.FindAsync(studentSubjectId);

            if (homework == null || subject == null)
            {
                return NotFound();
            }

            return View(new StudentHomeWork
            {
                HomeWork = homework,
                StudentSubject = subject
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentHomeWork model)
        {
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid();
                model.AnswerDateTime = DateTime.UtcNow;

                _context.Add(model);

                await _context.SaveChangesAsync();

                var homework = await _context.StudentHomeWorks
                    .Include(shw => shw.StudentSubject)
                    .ThenInclude(ssb => ssb.Subject)
                    .FirstOrDefaultAsync(shw => shw.Id == model.Id);

                return RedirectToAction("Details", "Subjects", new {id = homework.StudentSubject.SubjectId});
            }

            return View(model);
        }

        // GET: StudentHomeWorks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentHomeWork = await _context.StudentHomeWorks
                .Include(ssh => ssh.HomeWork)
                .Include(ssh => ssh.StudentSubject)
                .FirstOrDefaultAsync(ssh => ssh.Id == id);

            if (studentHomeWork == null)
            {
                return NotFound();
            }

            return View(studentHomeWork);
        }

        [
            HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentHomeWork model)
        {
            var studentHomeWork = await _context.StudentHomeWorks.FindAsync(id);

            if (id != model.Id || studentHomeWork == null || studentHomeWork.IsAccepted)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                studentHomeWork.StudentAnswer = model.StudentAnswer;
                studentHomeWork.IsChecked = false;

                _context.Update(studentHomeWork);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new {studentHomeWork.Id});
            }

            return View(studentHomeWork);
        }
    }
}