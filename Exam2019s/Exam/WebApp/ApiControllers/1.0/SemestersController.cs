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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student, Teacher, Admin")]
    public class SemestersController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <inheritdoc />
        public SemestersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<SemesterDTO>))]
        public async Task<IActionResult> Index()
        {
            var semesters = await _context.Semesters
                .Where(semester => semester.DeletedAt == null)
                .Select(semester => new SemesterDTO
                {
                    Id = semester.Id,
                    Title = semester.Title,
                    Subjects = semester.Subjects
                        .Where(subject => subject.DeletedAt == null)
                        .SelectMany(subject => subject.StudentSubjects)
                        .Where(subject => subject.DeletedAt == null && subject.StudentId == User.UserId())
                        .Select(subject => new SemesterSubjectDTO()
                        {
                            Id = subject.SubjectId,
                            Grade = subject.Grade,
                            SubjectCode = subject.Subject.SubjectCode,
                            SubjectTitle = subject.Subject.SubjectTitle,
                            TeacherName = subject.Subject.Teacher.FirstName + " " + subject.Subject.Teacher.LastName,
                            IsAccepted = subject.IsAccepted
                        }).ToList()
                }).ToListAsync();

            return Ok(semesters);
        }
    }
}