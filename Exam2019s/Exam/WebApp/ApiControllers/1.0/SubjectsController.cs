using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.EF;
using Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicApi.v1;

namespace WebApp.ApiControllers._1._0
{
    /// <inheritdoc />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <inheritdoc />
        public SubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<SubjectDTO>))]
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

            return Ok(subjects);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("my")]
        [Authorize(Roles = "Student, Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<SubjectDTO>))]
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

            return Ok(subjects);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [HttpPost("search")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<SubjectDTO>))]
        public async Task<IActionResult> Search(string? keywords)
        {
            if (string.IsNullOrEmpty(keywords))
            {
                return await Subjects();
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

            return Ok(subjects);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [HttpPost("search/my")]
        [Authorize(Roles = "Student, Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<SubjectDTO>))]
        public async Task<IActionResult> SearchStudent(string? keywords)
        {
            if (string.IsNullOrEmpty(keywords))
            {
                return await StudentSubjects();
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

            return Ok(subjects);
        }
        
        [AllowAnonymous]
        [HttpGet("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<SubjectDetails>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<SubjectStudentDetails>))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<SubjectTeacherDetails>))]
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
                return Ok(await query.Select(subject => new SubjectTeacherDetails
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
                            .Select(work => new SubjectTeacherDetailsHomework
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
                return Ok(await query.Select(subject => new SubjectStudentDetails
                    {
                        Id = subject.Id,
                        Grade = subject.StudentSubjects.FirstOrDefault(ssb => ssb.DeletedAt == null && ssb.IsAccepted && ssb.StudentId == User.UserId())
                            .Grade,
                        HomeWorksGrade = subject.StudentSubjects
                            .FirstOrDefault(ssb => ssb.DeletedAt == null && ssb.IsAccepted && ssb.StudentId == User.UserId()).StudentHomeWorks
                            .Where(shw => shw.DeletedAt == null)
                            .Select(work => work.Grade).Average(),
                        StudentsCount = subject.StudentSubjects.Count(s => s.DeletedAt == null && s.IsAccepted),
                        SemesterTitle = subject.Semester.Title,
                        SubjectCode = subject.SubjectCode,
                        SubjectTitle = subject.SubjectTitle,
                        TeacherName = subject.Teacher.FirstName + " " + subject.Teacher.LastName,
                        IsAccepted = subject.StudentSubjects.FirstOrDefault(ssb => ssb.DeletedAt == null && ssb.StudentId == User.UserId()).IsAccepted,
                        IsEnrolled = subject.StudentSubjects.Any(ssb => ssb.DeletedAt == null && ssb.StudentId == User.UserId()),
                        Homeworks = subject.HomeWorks
                            .Where(work => work.DeletedAt == null)
                            .Select(work => new SubjectStudentDetailsHomework
                            {
                                StudentHomeworkId = work.StudentHomeWorks.FirstOrDefault(shw =>
                                    shw.DeletedAt == null && shw.StudentSubject.StudentId == User.UserId()).Id,
                                Deadline = work.Deadline,
                                Grade = work.StudentHomeWorks.FirstOrDefault(shw =>
                                    shw.DeletedAt == null && shw.StudentSubject.StudentId == User.UserId()).Grade,
                                Id = work.Id,
                                Title = work.Title,
                                IsAccepted = work.StudentHomeWorks.FirstOrDefault(shw =>
                                    shw.DeletedAt == null && shw.StudentSubject.StudentId == User.UserId()).IsAccepted,
                                IsChecked = work.StudentHomeWorks.FirstOrDefault(shw =>
                                    shw.DeletedAt == null && shw.StudentSubject.StudentId == User.UserId()).IsChecked,
                                IsStarted = work.StudentHomeWorks.FirstOrDefault(shw =>
                                    shw.DeletedAt == null && shw.StudentSubject.StudentId == User.UserId()) != null
                            }).ToList()
                    })
                    .FirstOrDefaultAsync()
                );
            }

            return Ok(await query.Select(subject => new SubjectDetails
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