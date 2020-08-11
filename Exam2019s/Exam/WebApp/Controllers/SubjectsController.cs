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
    [Authorize]
    [Route("{controller}/{action=Subjects}")]
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Subjects()
        {
            var applicationDbContext = _context.Subjects
                .Include(s => s.Semester)
                .Include(s => s.Teacher)
                .Where(subject => subject.DeletedAt == null);
            return View(await applicationDbContext.ToListAsync());
        }

        [Route("/{controller}/my")]
        public async Task<IActionResult> StudentSubjects()
        {
            var applicationDbContext = _context.Subjects
                .Include(s => s.Semester)
                .Include(s => s.Teacher)
                .Where(subject => subject.DeletedAt == null)
                .Where(subject => subject.StudentSubject.Select(studentSubject => studentSubject.StudentId)
                    .Contains(User.UserId()));

            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Search(string? keywords)
        {
            if (string.IsNullOrEmpty(keywords))
            {
                return RedirectToAction("Subjects");
            }

            var applicationDbContext = _context.Subjects
                .Include(s => s.Semester)
                .Include(s => s.Teacher)
                .Where(subject => subject.DeletedAt == null)
                .Where(subject =>
                    subject.Teacher.FirstName.ToLower().Contains(keywords) ||
                    subject.Teacher.LastName.ToLower().Contains(keywords) ||
                    subject.Semester.Code.ToLower().Contains(keywords) ||
                    subject.Semester.Title.ToLower().Contains(keywords) ||
                    subject.SubjectTitle.ToLower().Contains(keywords) ||
                    subject.SubjectCode.ToLower().Contains(keywords));

            return View("Subjects", await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SearchStudent(string? keywords)
        {
            if (string.IsNullOrEmpty(keywords))
            {
                return RedirectToAction("StudentSubjects");
            }

            var applicationDbContext = _context.Subjects
                .Include(s => s.Semester)
                .Include(s => s.Teacher)
                .Where(subject => subject.DeletedAt == null)
                .Where(subject => subject.StudentSubject.Select(studentSubject => studentSubject.StudentId)
                    .Contains(User.UserId()))
                .Where(subject =>
                    subject.Teacher.FirstName.ToLower().Contains(keywords) ||
                    subject.Teacher.LastName.ToLower().Contains(keywords) ||
                    subject.Semester.Code.ToLower().Contains(keywords) ||
                    subject.Semester.Title.ToLower().Contains(keywords) ||
                    subject.SubjectTitle.ToLower().Contains(keywords) ||
                    subject.SubjectCode.ToLower().Contains(keywords));

            return View("StudentSubjects", await applicationDbContext.ToListAsync());
        }

        [Route("/{controller}/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Semester)
                .Include(s => s.Teacher)
                .Include(s => s.HomeWorks)
                .ThenInclude(s => s.StudentHomeWorks)
                .Include(s => s.StudentSubject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            subject.HomeWorks = subject.HomeWorks.Where(work => work.DeletedAt == null).ToList();

            return View("TeacherDetails", subject);
        }
    }
}