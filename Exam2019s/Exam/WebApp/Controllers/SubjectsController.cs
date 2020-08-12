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
using Resources.Views.Identity;

namespace WebApp.Controllers
{
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

        [Authorize(Roles = "Student, Teacher, Admin")]
        [Route("/{controller}/my")]
        public async Task<IActionResult> StudentSubjects()
        {
            var query = _context.Subjects
                .Include(s => s.Semester)
                .Include(s => s.Teacher)
                .Where(subject => subject.DeletedAt == null);

            if (User.IsInRole("Teacher"))
            {
                return View(await query.Where(subject => subject.TeacherId == User.UserId()).ToListAsync());
            }

            return View(await query.Where(subject => subject.StudentSubjects
                .Select(studentSubject => studentSubject.StudentId)
                .Contains(User.UserId())).ToListAsync());
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
        [Authorize(Roles = "Student, Teacher, Admin")]
        public async Task<IActionResult> SearchStudent(string? keywords)
        {
            if (string.IsNullOrEmpty(keywords))
            {
                return RedirectToAction("StudentSubjects");
            }

            var query = _context.Subjects
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

            if (User.IsInRole("Teacher"))
            {
                return View("StudentSubjects",
                    await query.Where(subject => subject.TeacherId == User.UserId()).ToListAsync());
            }

            return View("StudentSubjects", await query.Where(subject => subject.StudentSubjects
                .Select(studentSubject => studentSubject.StudentId)
                .Contains(User.UserId())).ToListAsync());
        }

        [Route("/{controller}/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Semester)
                .Include(s => s.Teacher)
                .Include(s => s.HomeWorks)
                .ThenInclude(s => s.StudentHomeWorks)
                .Include(s => s.StudentSubjects)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (subject == null)
            {
                return NotFound();
            }

            subject.HomeWorks = subject.HomeWorks.Where(work => work.DeletedAt == null).ToList();

            if (User.IsInRole("Teacher") && subject.TeacherId == User.UserId())
            {
                return View("/Views/Subjects/TeacherDetails.cshtml", subject);
            }
            
            if (User.IsInRole("Student"))
            {
                return View("/Views/Subjects/StudentDetails.cshtml", subject);
            }

            return View("/Views/Subjects/Details.cshtml", subject);
        }
    }
}