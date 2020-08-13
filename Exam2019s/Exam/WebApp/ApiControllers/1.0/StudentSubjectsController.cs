using System;
using System.Collections.Generic;
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
    public class StudentSubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <inheritdoc />
        public StudentSubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpGet("{subjectId}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<StudentSubjectDTO>))]
        public async Task<IActionResult> Index(Guid subjectId)
        {
            return Ok(await _context.StudentSubjects
                .Where(subject => subject.SubjectId == subjectId &&
                                  subject.DeletedAt == null)
                .Select(ssb => new StudentSubjectDTO
                {
                    Grade = ssb.Grade,
                    Id = ssb.Id,
                    IsAccepted = ssb.IsAccepted,
                    StudentCode = ssb.Student.UserName,
                    StudentName = ssb.Student.FirstName + " " + ssb.Student.LastName,
                    SubjectId = ssb.SubjectId
                }).ToListAsync()
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("editmodel/{id}")]
        [Authorize(Roles = "Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentSubjectDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentSubject = await _context.StudentSubjects
                .Where(subject => subject.Id == id &&
                                  subject.DeletedAt == null)
                .Select(ssb => new StudentSubjectDTO
                {
                    Grade = ssb.Grade,
                    Id = ssb.Id,
                    IsAccepted = ssb.IsAccepted,
                    StudentCode = ssb.Student.UserName,
                    StudentName = ssb.Student.FirstName + " " + ssb.Student.LastName,
                    SubjectId = ssb.SubjectId
                }).FirstOrDefaultAsync();

            if (studentSubject == null)
            {
                return NotFound();
            }

            return Ok(studentSubject);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit(StudentSubjectPutDTO model)
        {
            var subject = await _context.StudentSubjects
                .FirstOrDefaultAsync(s => s.Id == model.Id);
            
            if (User.IsInRole("Teacher") && 
                !await _context.StudentSubjects.AnyAsync(s => s.Id == model.Id && s.Subject.TeacherId == User.UserId()))
            {
                return BadRequest();
            }

            subject.Grade = model.Grade;
            subject.IsAccepted = model.IsAccepted;

            if (ModelState.IsValid)
            {
                _context.StudentSubjects.Update(subject);
                await _context.SaveChangesAsync();

                return NoContent();
            }


            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("student/new")]
        [Authorize(Roles = "Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptStudent(StudentControlDTO model)
        {
            var studentSubject =
                await _context.StudentSubjects
                    .FirstOrDefaultAsync(s =>
                        s.Id == model.Id && s.DeletedAt == null);

            if (studentSubject == null || studentSubject.IsAccepted)
            {
                return BadRequest();
            }

            studentSubject.IsAccepted = true;

            _context.StudentSubjects.Update(studentSubject);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("student/remove")]
        [Authorize(Roles = "Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveStudent(StudentControlDTO model)
        {
            var studentSubject =
                await _context.StudentSubjects
                    .FirstOrDefaultAsync(s =>
                        s.Id == model.Id && s.DeletedAt == null);

            if (studentSubject == null)
            {
                return BadRequest();
            }

            studentSubject.IsAccepted = false;
            studentSubject.Grade = -1;
            
            studentSubject.DeletedAt = DateTime.UtcNow;
            studentSubject.DeletedBy = User.Identity.Name;
            
            _context.RemoveRange(_context.StudentHomeWorks.Where(ssh => ssh.StudentSubjectId == studentSubject.Id));

            _context.StudentSubjects.Update(studentSubject);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpPost("subject/register")]
        [Authorize(Roles = "Student, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterToSubject(Guid subjectId)
        {
            var studentSubject =
                await _context.StudentSubjects
                    .FirstOrDefaultAsync(s =>
                        s.StudentId == User.UserId() &&
                        s.SubjectId == subjectId);

            if (studentSubject == null)
            {
                _context.StudentSubjects.Add(new StudentSubject
                {
                    StudentId = User.UserId(),
                    SubjectId = subjectId,
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

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        [HttpPost("subject/unregister")]
        [Authorize(Roles = "Student, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelRegistration(Guid subjectId)
        {
            var subject = await _context.StudentSubjects
                .FirstOrDefaultAsync(s =>
                    s.DeletedAt == null &&
                    s.StudentId == User.UserId() &&
                    s.SubjectId == subjectId);

            if (subject == null || subject.IsAccepted)
            {
                return BadRequest();
            }

            subject.IsAccepted = false;

            _context.StudentSubjects.Remove(subject);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}