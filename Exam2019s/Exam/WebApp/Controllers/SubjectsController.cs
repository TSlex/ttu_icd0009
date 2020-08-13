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

//        [Route("/{controller}/{id}")]
//        public async Task<IActionResult> Details(Guid id)
//        {
//            var subject = await _context.Subjects
//                .Include(s => s.Semester)
//                .Include(s => s.Teacher)
//                .Include(s => s.HomeWorks)
//                .ThenInclude(s => s.StudentHomeWorks)
//                .Include(s => s.StudentSubjects)
//                .FirstOrDefaultAsync(m => m.Id == id);
//
//            if (subject == null)
//            {
//                return NotFound();
//            }
//
//            subject.HomeWorks = subject.HomeWorks.Where(work => work.DeletedAt == null).ToList();
//            subject.StudentSubjects = subject.StudentSubjects.Where(ssb => ssb.DeletedAt == null).ToList();
//
//            if (User.IsInRole("Teacher") && subject.TeacherId == User.UserId() || User.IsInRole("Admin"))
//            {
//                return View("/Views/Subjects/TeacherDetails.cshtml", subject);
//            }
//
//            if (User.IsInRole("Student"))
//            {
//                return View("/Views/Subjects/StudentDetails.cshtml", subject);
//            }
//
//            return View("/Views/Subjects/Details.cshtml", subject);
//        }

        public async Task<IActionResult> Details(Guid id)
        {
            var record = await _context.Subjects.FindAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            var query = _context.Subjects.Where(s => s.Id == id && s.DeletedAt == null);

            if (User.IsInRole("Teacher") && record.TeacherId == User.UserId() || User.IsInRole("Admin"))
            {
                return View("/Views/Subjects/TeacherDetails.cshtml", await query.Select(subject => new SubjectTeacherDetailsDTO
                    {
                        Id = subject.Id,
                        NotGradedCount = subject.StudentSubjects.Where(s => s.IsAccepted && s.DeletedAt == null)
                            .Count(s => s.Grade < 0),
                        FailedCount = subject.StudentSubjects.Where(s => s.IsAccepted && s.DeletedAt == null)
                            .Count(s => s.Grade == 0),
                        PassedCount = subject.StudentSubjects.Where(s => s.IsAccepted && s.DeletedAt == null)
                            .Count(s => s.Grade > 0),
                        Score1Count = subject.StudentSubjects.Where(s => s.IsAccepted && s.DeletedAt == null)
                            .Count(s => s.Grade == 1),
                        Score2Count = subject.StudentSubjects.Where(s => s.IsAccepted && s.DeletedAt == null)
                            .Count(s => s.Grade == 2),
                        Score3Count = subject.StudentSubjects.Where(s => s.IsAccepted && s.DeletedAt == null)
                            .Count(s => s.Grade == 3),
                        Score4Count = subject.StudentSubjects.Where(s => s.IsAccepted && s.DeletedAt == null)
                            .Count(s => s.Grade == 4),
                        Score5Count = subject.StudentSubjects.Where(s => s.IsAccepted && s.DeletedAt == null)
                            .Count(s => s.Grade == 5),
                        AcceptedStudentsCount = subject.StudentSubjects.Count(s => s.IsAccepted && s.DeletedAt == null),
                        NotAcceptedStudentsCount =
                            subject.StudentSubjects.Count(s => !s.IsAccepted && s.DeletedAt == null),
                        SemesterTitle = subject.Semester.Title,
                        SubjectCode = subject.SubjectCode,
                        SubjectTitle = subject.SubjectTitle,
                        TeacherName = subject.Teacher.FirstName + " " + subject.Teacher.LastName,
                        Homeworks = subject.HomeWorks
                            .Where(work => work.DeletedAt == null)
                            .Select(work => new SubjectTeacherDetailsHomeworkDTO
                            {
                                Deadline = work.Deadline,
                                AverageGrade = work.StudentHomeWorks
                                    .Where(homeWork => homeWork.DeletedAt == null && homeWork.Grade > 0)
                                    .Select(homeWork => homeWork.Grade).Average(),
                                Id = work.Id,
                                Title = work.Title
                            }).ToList()
                    })
                    .FirstOrDefaultAsync());
            }

            if (User.IsInRole("Student"))
            {
                return View("/Views/Subjects/StudentDetails.cshtml", await query.Select(subject => new SubjectStudentDetailsDTO
                    {
                        Id = subject.Id,
                        StudentSubjectId = subject.StudentSubjects.FirstOrDefault(ssb =>
                                ssb.DeletedAt == null && ssb.IsAccepted && ssb.StudentId == User.UserId())
                            .Id,
                        Grade = subject.StudentSubjects.FirstOrDefault(ssb =>
                                ssb.DeletedAt == null && ssb.IsAccepted && ssb.StudentId == User.UserId())
                            .Grade,
                        HomeWorksGrade = subject.StudentSubjects
                            .FirstOrDefault(ssb =>
                                ssb.DeletedAt == null && ssb.IsAccepted && ssb.StudentId == User.UserId())
                            .StudentHomeWorks
                            .Where(shw => shw.DeletedAt == null && shw.Grade >= 0)
                            .Select(work => work.Grade).Average(),
                        StudentsCount = subject.StudentSubjects.Count(s => s.DeletedAt == null && s.IsAccepted),
                        SemesterTitle = subject.Semester.Title,
                        SubjectCode = subject.SubjectCode,
                        SubjectTitle = subject.SubjectTitle,
                        TeacherName = subject.Teacher.FirstName + " " + subject.Teacher.LastName,
                        IsAccepted = subject.StudentSubjects
                            .FirstOrDefault(ssb => ssb.DeletedAt == null && ssb.StudentId == User.UserId()).IsAccepted,
                        IsEnrolled = subject.StudentSubjects.Any(ssb =>
                            ssb.DeletedAt == null && ssb.StudentId == User.UserId()),
                        Homeworks = subject.HomeWorks
                            .Where(work => work.DeletedAt == null)
                            .Select(work => new SubjectStudentDetailsHomeworkDTO
                            {
                                StudentHomeworkId = work.StudentHomeWorks.FirstOrDefault(shw =>
                                    shw.DeletedAt == null && shw.StudentSubject.StudentId == User.UserId()).Id,
                                Deadline = work.Deadline,
                                Grade = work.StudentHomeWorks.FirstOrDefault(shw =>
                                                shw.DeletedAt == null && shw.StudentSubject.StudentId == User.UserId())
                                            .Grade >= 0
                                    ? work.StudentHomeWorks.FirstOrDefault(shw =>
                                        shw.DeletedAt == null && shw.StudentSubject.StudentId == User.UserId()).Grade
                                    : -1,
                                Id = work.Id,
                                Title = work.Title,
                                IsAccepted = work.StudentHomeWorks.FirstOrDefault(shw =>
                                    shw.DeletedAt == null && shw.StudentSubject.StudentId == User.UserId()).IsAccepted,
                                IsChecked = work.StudentHomeWorks.FirstOrDefault(shw =>
                                    shw.DeletedAt == null && shw.StudentSubject.StudentId == User.UserId()).IsChecked,
                                IsStarted = work.StudentHomeWorks.FirstOrDefault(shw =>
                                                shw.DeletedAt == null &&
                                                shw.StudentSubject.StudentId == User.UserId()) != null
                            }).ToList()
                    })
                    .FirstOrDefaultAsync()
                );
            }

            return View("/Views/Subjects/Details.cshtml", await query.Select(subject => new SubjectDetailsDTO
            {
                Id = subject.Id,
                StudentsCount = subject.StudentSubjects.Count(s => s.DeletedAt == null && s.IsAccepted),
                SemesterTitle = subject.Semester.Title,
                SubjectCode = subject.SubjectCode,
                SubjectTitle = subject.SubjectTitle,
                TeacherName = subject.Teacher.FirstName + " " + subject.Teacher.LastName,
            }).FirstOrDefaultAsync());
        }
    }
}