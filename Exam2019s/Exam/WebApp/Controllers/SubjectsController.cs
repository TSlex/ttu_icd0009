using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
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
            var subjects = await _context.Subjects
                .Where(subject => subject.DeletedAt == null)
                .Select(subject => new SubjectDTO
                {
                    Id = subject.Id,
                    SemesterTitle = subject.Semester.Title,
                    SubjectCode = subject.SubjectCode,
                    SubjectTitle = subject.SubjectTitle,
                    TeacherName = subject.Teacher.FirstName + " " + subject.Teacher.LastName
                }).ToListAsync();

            return View(subjects);
        }

        [Authorize(Roles = "Student, Teacher, Admin")]
        [Route("/{controller}/my")]
        public async Task<IActionResult> StudentSubjects()
        {
            var query = _context.Subjects
                .Where(subject => subject.DeletedAt == null);

            ICollection<SubjectDTO> subjects;

            if (User.IsInRole("Teacher"))
            {
                subjects = await query
                    .Where(subject => subject.Teacher.Id == User.UserId())
                    .Select(subject => new SubjectDTO
                    {
                        Id = subject.Id,
                        SemesterTitle = subject.Semester.Title,
                        SubjectCode = subject.SubjectCode,
                        SubjectTitle = subject.SubjectTitle,
                        TeacherName = subject.Teacher.FirstName + " " + subject.Teacher.LastName
                    }).ToListAsync();
            }
            else
            {
                subjects = await query
                    .SelectMany(subject => subject.StudentSubjects)
                    .Select(subject => subject)
                    .Where(subject => subject.StudentId == User.UserId() && subject.DeletedAt == null)
                    .Select(subject => new SubjectDTO
                    {
                        Id = subject.Subject.Id,
                        SemesterTitle = subject.Subject.Semester.Title,
                        SubjectCode = subject.Subject.SubjectCode,
                        SubjectTitle = subject.Subject.SubjectTitle,
                        TeacherName = subject.Subject.Teacher.FirstName + " " + subject.Subject.Teacher.LastName
                    }).ToListAsync();
            }

            return View(subjects);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string? keywords)
        {
            if (string.IsNullOrEmpty(keywords))
            {
                return RedirectToAction("Subjects");
            }

            var query = _context.Subjects
                .Where(subject => subject.DeletedAt == null)
                .Where(subject =>
                    subject.Teacher.FirstName.ToLower().Contains(keywords.ToLower()) ||
                    subject.Teacher.LastName.ToLower().Contains(keywords.ToLower()) ||
                    subject.Semester.Code.ToLower().Contains(keywords.ToLower()) ||
                    subject.Semester.Title.ToLower().Contains(keywords.ToLower()) ||
                    subject.SubjectTitle.ToLower().Contains(keywords.ToLower()) ||
                    subject.SubjectCode.ToLower().Contains(keywords.ToLower()));

            var subjects = await query
                .Select(subject => new SubjectDTO
                {
                    Id = subject.Id,
                    SemesterTitle = subject.Semester.Title,
                    SubjectCode = subject.SubjectCode,
                    SubjectTitle = subject.SubjectTitle,
                    TeacherName = subject.Teacher.FirstName + " " + subject.Teacher.LastName
                }).ToListAsync();

            return View("Subjects", subjects);
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
                .Where(subject => subject.DeletedAt == null)
                .Where(subject =>
                    subject.Teacher.FirstName.ToLower().Contains(keywords.ToLower()) ||
                    subject.Teacher.LastName.ToLower().Contains(keywords.ToLower()) ||
                    subject.Semester.Code.ToLower().Contains(keywords.ToLower()) ||
                    subject.Semester.Title.ToLower().Contains(keywords.ToLower()) ||
                    subject.SubjectTitle.ToLower().Contains(keywords.ToLower()) ||
                    subject.SubjectCode.ToLower().Contains(keywords.ToLower()));

            ICollection<SubjectDTO> subjects;

            if (User.IsInRole("Teacher"))
            {
                subjects = await query
                    .Where(subject => subject.Teacher.Id == User.UserId())
                    .Select(subject => new SubjectDTO
                    {
                        Id = subject.Id,
                        SemesterTitle = subject.Semester.Title,
                        SubjectCode = subject.SubjectCode,
                        SubjectTitle = subject.SubjectTitle,
                        TeacherName = subject.Teacher.FirstName + " " + subject.Teacher.LastName
                    }).ToListAsync();
            }
            else
            {
                subjects = await query
                    .SelectMany(subject => subject.StudentSubjects)
                    .Select(subject => subject)
                    .Where(subject => subject.StudentId == User.UserId() && subject.DeletedAt == null)
                    .Select(subject => new SubjectDTO
                    {
                        Id = subject.Subject.Id,
                        SemesterTitle = subject.Subject.Semester.Title,
                        SubjectCode = subject.Subject.SubjectCode,
                        SubjectTitle = subject.Subject.SubjectTitle,
                        TeacherName = subject.Subject.Teacher.FirstName + " " + subject.Subject.Teacher.LastName
                    }).ToListAsync();
            }

            return View("StudentSubjects", subjects);
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
            subject.StudentSubjects = subject.StudentSubjects.Where(ssb => ssb.DeletedAt == null).ToList();

            if (User.IsInRole("Teacher") && subject.TeacherId == User.UserId() || User.IsInRole("Admin"))
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