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
    public class StudentSubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentSubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Guid subjectId)
        {
            var applicationDbContext = _context.StudentSubjects
                .Include(s => s.Student)
                .Where(subject => subject.SubjectId == subjectId &&
                                  subject.DeletedAt == null);

            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Teacher, Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentSubject = await _context.StudentSubjects
                .Include(subject => subject.Student)
                .FirstOrDefaultAsync(subject => subject.Id == id);

            if (studentSubject == null)
            {
                return NotFound();
            }

            return View(studentSubject);
        }


        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentSubject model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var subject = await _context.StudentSubjects
                .FirstOrDefaultAsync(s => s.Id == id);

            subject.Grade = model.Grade;
            subject.IsAccepted = model.IsAccepted;

            if (ModelState.IsValid)
            {
                _context.StudentSubjects.Update(subject);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new {subject.SubjectId});
            }


            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<IActionResult> AcceptStudent(StudentSubject model)
        {
            var studentSubject =
                await _context.StudentSubjects
                    .FirstOrDefaultAsync(s =>
                        s.Id == model.Id && s.DeletedAt == null);

            if (studentSubject == null || studentSubject.IsAccepted)
            {
                return RedirectToAction("Index", new {model.SubjectId});
            }

            studentSubject.IsAccepted = true;

            _context.StudentSubjects.Update(studentSubject);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new {model.SubjectId});
        }

        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<IActionResult> RemoveStudent(StudentSubject model)
        {
            var studentSubject =
                await _context.StudentSubjects
                    .FirstOrDefaultAsync(s =>
                        s.Id == model.Id && s.DeletedAt == null);

            if (studentSubject == null)
            {
                return RedirectToAction("Index", new {model.SubjectId});
            }

            studentSubject.IsAccepted = false;
            studentSubject.Grade = -1;
            
            studentSubject.DeletedAt = DateTime.UtcNow;
            studentSubject.DeletedBy = User.Identity.Name;
            
            _context.RemoveRange(_context.StudentHomeWorks.Where(ssh => ssh.StudentSubjectId == studentSubject.Id));

            _context.StudentSubjects.Update(studentSubject);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new {model.SubjectId});
        }

        [HttpPost]
        [Authorize(Roles = "Student, Admin")]
        public async Task<IActionResult> RegisterToSubject(Subject model)
        {
            var studentSubject =
                await _context.StudentSubjects
                    .FirstOrDefaultAsync(s =>
                        s.StudentId == User.UserId() &&
                        s.SubjectId == model.Id);

            if (studentSubject == null)
            {
                _context.StudentSubjects.Add(new StudentSubject
                {
                    StudentId = User.UserId(),
                    SubjectId = model.Id,
                });

                await _context.SaveChangesAsync();
            }
            else if (studentSubject.DeletedAt != null)
            {
                studentSubject.DeletedAt = null;
                studentSubject.DeletedBy = null;

                _context.StudentSubjects.Update(studentSubject);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Subjects", new {id = model.Id});
        }

        [HttpPost]
        [Authorize(Roles = "Student, Admin")]
        public async Task<IActionResult> CancelRegistration(Subject model)
        {
            var subject = await _context.StudentSubjects
                .FirstOrDefaultAsync(s =>
                    s.DeletedAt == null &&
                    s.StudentId == User.UserId() &&
                    s.SubjectId == model.Id);

            if (subject == null || subject.IsAccepted)
            {
                return RedirectToAction("Subjects", "Subjects");
            }

            subject.IsAccepted = false;

            _context.StudentSubjects.Remove(subject);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Subjects", new {id = model.Id});
        }
    }
}