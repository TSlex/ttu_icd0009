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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student, Teacher, Admin")]
    public class SemestersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SemestersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Semesters
                .Include(semester => semester.Subjects)
                .ThenInclude(subject => subject.Teacher)
                .Include(semester => semester.Subjects)
                .ThenInclude(subject => subject.StudentSubjects)
                .Where(semester =>
                    semester.Subjects
                        .SelectMany(subject => subject.StudentSubjects)
                        .Select(ssb => ssb.StudentId).Contains(User.UserId()))
                .ToListAsync());
        }
    }
}