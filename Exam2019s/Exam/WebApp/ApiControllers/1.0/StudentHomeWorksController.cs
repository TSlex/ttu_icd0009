using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.EF;
using Domain;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student, Teacher, Admin")]
    public class StudentHomeWorksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentHomeWorksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Consumes(("application/json"))]
        [Produces("application/json")]
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

        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> Details(Guid id)
        {
            var studentHomeWork = await _context.StudentHomeWorks
                .Include(s => s.HomeWork)
                .ThenInclude(hw => hw.Subject)
                .Include(s => s.StudentSubject)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (studentHomeWork == null)
            {
                return NotFound();
            }

            return View(studentHomeWork);
        }

        [Consumes(("application/json"))]
        [Produces("application/json")]
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
        [Consumes(("application/json"))]
        [Produces("application/json")]
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

        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentHomeWork = await _context.StudentHomeWorks
                .Include(ssh => ssh.HomeWork)
                .ThenInclude(hw => hw.Subject)
                .Include(ssh => ssh.StudentSubject)
                .FirstOrDefaultAsync(ssh => ssh.Id == id);

            if (studentHomeWork == null)
            {
                return NotFound();
            }

            return View(studentHomeWork);
        }

        [HttpPost]
        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> Edit(Guid id, StudentHomeWork model)
        {
            var studentHomeWork = await _context.StudentHomeWorks.FindAsync(id);

            if (id != model.Id || studentHomeWork == null || studentHomeWork.IsAccepted)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                studentHomeWork.AnswerDateTime = DateTime.UtcNow;
                studentHomeWork.StudentAnswer = model.StudentAnswer;
                studentHomeWork.IsChecked = false;

                _context.Update(studentHomeWork);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new {studentHomeWork.Id});
            }

            return View(studentHomeWork);
        }
        
        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> TeacherSubmit(Guid homeworkId, Guid studentSubjectId)
        {
            var studentHomeWork = await _context.StudentHomeWorks
                .Include(ssh => ssh.HomeWork)
                .ThenInclude(hw => hw.Subject)
                .Include(ssh => ssh.StudentSubject)
                .FirstOrDefaultAsync(work =>
                    work.HomeWorkId == homeworkId && work.StudentSubjectId == studentSubjectId);

            if (studentHomeWork == null)
            {
                var id = Guid.NewGuid();
                
                _context.Add(new StudentHomeWork()
                {
                    Id = id,
                    HomeWorkId = homeworkId,
                    StudentSubjectId = studentSubjectId
                });

                await _context.SaveChangesAsync();
                
                studentHomeWork = await _context.StudentHomeWorks
                    .Include(ssh => ssh.HomeWork)
                    .ThenInclude(hw => hw.Subject)
                    .Include(ssh => ssh.StudentSubject)
                    .FirstOrDefaultAsync(ssh => ssh.Id == id);
            }

            return View("TeacherDetails", studentHomeWork);
        }
        
        [HttpPost]
        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> TeacherSubmit(Guid id, StudentHomeWork model)
        {
            var studentHomeWork = await _context.StudentHomeWorks.FindAsync(id);

            if (id != model.Id || studentHomeWork == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                studentHomeWork.Grade = model.Grade;
                studentHomeWork.IsAccepted = model.IsAccepted;
                studentHomeWork.IsChecked = model.IsChecked;

                _context.Update(studentHomeWork);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "HomeWorks", new {id = studentHomeWork.HomeWorkId});
            }

            return View("TeacherDetails", studentHomeWork);
        }
    }
}