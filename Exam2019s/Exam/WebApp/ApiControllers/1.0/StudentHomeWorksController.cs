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
using Microsoft.EntityFrameworkCore.Internal;
using PublicApi.v1;
using StudentHomeWork = Domain.StudentHomeWork;

namespace WebApp.ApiControllers._1._0
{
    /// <inheritdoc />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student, Teacher, Admin")]
    public class StudentHomeWorksController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <inheritdoc />
        public StudentHomeWorksController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentHomeWorkDetailsDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Details(Guid id)
        {
            var studentHomeWork = await _context.StudentHomeWorks
                .Where(shw => shw.Id == id && shw.DeletedAt == null)
                .Select(shw => new StudentHomeWorkDetailsDTO
                {
                    Deadline = shw.HomeWork.Deadline,
                    Description = shw.HomeWork.Description,
                    Title = shw.HomeWork.Title,
                    Grade = shw.Grade,
                    IsAccepted = shw.IsAccepted,
                    IsChecked = shw.IsChecked,
                    StudentAnswer = shw.StudentAnswer,
                    SubjectCode = shw.HomeWork.Subject.SubjectCode,
                    SubjectId = shw.HomeWork.Subject.Id,
                    SubjectTitle = shw.HomeWork.Subject.SubjectTitle,
                    AnswerDateTime = shw.AnswerDateTime,
                    HomeWorkId = shw.HomeWork.Id
                }).FirstOrDefaultAsync();

            if (studentHomeWork == null)
            {
                return NotFound();
            }

            return Ok(studentHomeWork);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Student, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(StudentHomeWorkPostDTO model)
        {
            if (ModelState.IsValid)
            {
                var homework = new StudentHomeWork
                {
                    Id = Guid.NewGuid(),
                    StudentAnswer = model.StudentAnswer,
                    HomeWorkId = model.HomeWorkId,
                    StudentSubjectId = model.StudentSubjectId,
                    AnswerDateTime = DateTime.UtcNow
                };

                _context.Add(homework);

                await _context.SaveChangesAsync();

                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "Student, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

                return NoContent();
            }

            return BadRequest();
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpGet("teacher/{homeworkId}/{studentSubjectId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

            return Ok();
        }

        [HttpPut("teacher")]
        [Authorize(Roles = "Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

            return NoContent();
        }
    }
}