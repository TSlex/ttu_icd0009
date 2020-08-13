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
        /// <returns></returns>
        [HttpGet("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentHomeWorkDetailsDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Details(Guid id)
        {
            var studentHomeWork = await _context.StudentHomeWorks
                .Where(shw =>
                    shw.Id == id && shw.DeletedAt == null)
                .Select(shw => new StudentHomeWorkDetailsDTO
                {
                    Id = shw.Id,
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
                var homework = new Domain.StudentHomeWork
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Student, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Edit(StudentHomeWorkPutDTO model)
        {
            var studentHomeWork = await _context.StudentHomeWorks.FindAsync(model.Id);

            if (studentHomeWork == null || studentHomeWork.IsAccepted)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="homeworkId"></param>
        /// <param name="studentSubjectId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Teacher, Admin")]
        [HttpGet("{homeworkId}/{studentSubjectId}/teacher")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentHomeWorkDetailsDTO))]
        public async Task<IActionResult> TeacherSubmit(Guid homeworkId, Guid studentSubjectId)
        {
            var query = _context.StudentHomeWorks
                .Where(shw =>
                    shw.StudentSubjectId == studentSubjectId && shw.HomeWorkId == homeworkId && shw.DeletedAt == null)
                .Select(shw => new StudentHomeWorkDetailsDTO
                {
                    Id = shw.Id,
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
                });

            var studentHomeWork = await query.FirstOrDefaultAsync();

            if (studentHomeWork == null)
            {
                var id = Guid.NewGuid();

                _context.Add(new Domain.StudentHomeWork()
                {
                    Id = id,
                    HomeWorkId = homeworkId,
                    StudentSubjectId = studentSubjectId
                });

                await _context.SaveChangesAsync();

                studentHomeWork = await query.FirstOrDefaultAsync();
            }

            return Ok(studentHomeWork);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("teacher")]
        [Authorize(Roles = "Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> TeacherSubmit(StudentHomeWorkTeacherSubmitDTO model)
        {
            var studentHomeWork = await _context.StudentHomeWorks.FindAsync(model.Id);

            if (studentHomeWork == null)
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

                return NoContent();
            }

            return BadRequest();
        }
    }
}