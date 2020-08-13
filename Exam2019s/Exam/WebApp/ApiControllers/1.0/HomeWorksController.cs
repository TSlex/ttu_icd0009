using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.EF;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicApi.v1;
using StudentHomeWork = PublicApi.v1.StudentHomeWork;

namespace WebApp.ApiControllers._1._0
{
    /// <inheritdoc />
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher, Admin, Student")]
    public class HomeWorksController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <inheritdoc />
        public HomeWorksController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HomeWorkDetailsDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Details(Guid id)
        {
            var homeWork = await _context.HomeWorks
                .Where(work => work.DeletedAt == null && work.Id == id)
                .Select(work => new HomeWorkDetailsDTO
                {
                    Id = work.Id,
                    SubjectId = work.SubjectId,
                    SubjectCode = work.Subject.SubjectCode,
                    SubjectTitle = work.Subject.SubjectTitle,
                    Deadline = work.Deadline,
                    Title = work.Title,
                    Description = work.Description,
                    StudentHomeWorks = work.Subject.StudentSubjects
                        .Where(ssb => ssb.DeletedAt == null && ssb.IsAccepted)
                        .Select(ssb => new StudentHomeWork
                        {
                            StudentSubjectId = ssb.Id,
                            SubjectId = ssb.SubjectId,
                            HomeWorkId = work.Id,
                            IsAccepted = ssb.StudentHomeWorks.FirstOrDefault(w => w.HomeWorkId == work.Id).IsAccepted,
                            IsChecked = ssb.StudentHomeWorks.FirstOrDefault(w => w.HomeWorkId == work.Id).IsChecked,
                            StudentCode = ssb.Student.UserName,
                            StudentName = ssb.Student.FirstName + " " + ssb.Student.LastName,
                            Grade = ssb.StudentHomeWorks.FirstOrDefault(w => w.HomeWorkId == work.Id).Grade
                        }).ToList()
                }).FirstOrDefaultAsync();

            if (homeWork == null)
            {
                return NotFound();
            }

            return Ok(homeWork);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(HomeWorkPostDTO model)
        {
            if (ModelState.IsValid)
            {
                var homeWork = new HomeWork()
                {
                    Id = Guid.NewGuid(),
                    Deadline = model.Deadline,
                    Description = model.Description,
                    Title = model.Title,
                    SubjectId = model.SubjectId,
                };

                _context.Add(homeWork);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("editmodel/{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize(Roles = "Teacher, Admin, Student")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HomeWorkDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeWork = await _context.HomeWorks
                .Where(work => work.DeletedAt == null && work.Id == id)
                .Select(work => new HomeWorkDTO
                {
                    Deadline = work.Deadline,
                    Description = work.Description,
                    Id = work.Id,
                    Title = work.Title,
                    SubjectId = work.SubjectId,
                    SubjectTitle = work.Subject.SubjectTitle,
                    SubjectCode = work.Subject.SubjectCode
                }).FirstOrDefaultAsync();

            if (homeWork == null)
            {
                return NotFound();
            }

            return Ok(homeWork);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="homeWork"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit(HomeWorkPutDTO homeWork)
        {
            var record = await _context.HomeWorks.FindAsync(homeWork.Id);

            if (ModelState.IsValid)
            {
                record.Title = homeWork.Title;
                record.Description = homeWork.Description;
                record.Deadline = homeWork.Deadline;

                _context.Update(record);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            return View(record);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher, Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var homeWork = await _context.HomeWorks.FindAsync(id);

            _context.HomeWorks.Remove(homeWork);
            
            //remove related student homeworks
            _context.RemoveRange(await _context.StudentHomeWorks.Where(shw => shw.HomeWorkId == homeWork.Id)
                .ToListAsync());

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Subjects", new {id = homeWork.SubjectId});
        }
    }
}