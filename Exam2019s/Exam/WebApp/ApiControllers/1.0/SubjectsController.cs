using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.EF;
using Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [AllowAnonymous]
        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> Subjects()
        {
            var applicationDbContext = _context.Subjects
                .Include(s => s.Semester)
                .Include(s => s.Teacher)
                .Where(subject => subject.DeletedAt == null);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet("my")]
        [Authorize(Roles = "Student, Teacher, Admin")]
        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> StudentSubjects()
        {
            var query = _context.Subjects
                .Include(s => s.Semester)
                .Include(s => s.Teacher)
                .Include(s => s.StudentSubjects)
                .Where(subject => subject.DeletedAt == null);

            if (User.IsInRole("Teacher"))
            {
                return View(await query.Where(subject => subject.TeacherId == User.UserId()).ToListAsync());
            }

            var studentSubjects = await query.Where(subject =>
                    subject.StudentSubjects
                        .Select(studentSubject => studentSubject.StudentId)
                        .Contains(User.UserId())
//                    &&
//                    subject.StudentSubjects
//                        .Select(studentSubject => studentSubject.DeletedAt)
//                        .Contains(null)
                )
                .ToListAsync();

            return View(studentSubjects);
        }

        [HttpPost("search")]
        [AllowAnonymous]
        [Consumes(("application/json"))]
        [Produces("application/json")]
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
                    subject.Teacher.FirstName.ToLower().Contains(keywords.ToLower()) ||
                    subject.Teacher.LastName.ToLower().Contains(keywords.ToLower()) ||
                    subject.Semester.Code.ToLower().Contains(keywords.ToLower()) ||
                    subject.Semester.Title.ToLower().Contains(keywords.ToLower()) ||
                    subject.SubjectTitle.ToLower().Contains(keywords.ToLower()) ||
                    subject.SubjectCode.ToLower().Contains(keywords.ToLower()));

            return View("Subjects", await applicationDbContext.ToListAsync());
        }

        [HttpPost("search/my")]
        [Authorize(Roles = "Student, Teacher, Admin")]
        [Consumes(("application/json"))]
        [Produces("application/json")]
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
                    subject.Teacher.FirstName.ToLower().Contains(keywords.ToLower()) ||
                    subject.Teacher.LastName.ToLower().Contains(keywords.ToLower()) ||
                    subject.Semester.Code.ToLower().Contains(keywords.ToLower()) ||
                    subject.Semester.Title.ToLower().Contains(keywords.ToLower()) ||
                    subject.SubjectTitle.ToLower().Contains(keywords.ToLower()) ||
                    subject.SubjectCode.ToLower().Contains(keywords.ToLower()));

            if (User.IsInRole("Teacher"))
            {
                return View("StudentSubjects",
                    await query.Where(subject => subject.TeacherId == User.UserId()).ToListAsync());
            }

            return View("StudentSubjects", await query.Where(subject => subject.StudentSubjects
                .Select(studentSubject => studentSubject.StudentId)
                .Contains(User.UserId())).ToListAsync());
        }

        [HttpGet("{id}")]
        [Consumes(("application/json"))]
        [Produces("application/json")]
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