using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.ApiControllers
{
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class RanksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RanksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Ranks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rank>>> GetRanks()
        {
            return await _context.Ranks.ToListAsync();
        }

        // GET: api/Ranks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rank>> GetRank(Guid id)
        {
            var rank = await _context.Ranks.FindAsync(id);

            if (rank == null)
            {
                return NotFound();
            }

            return rank;
        }

        // PUT: api/Ranks/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRank(Guid id, Rank rank)
        {
            if (id != rank.Id)
            {
                return BadRequest();
            }

            _context.Entry(rank).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RankExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ranks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Rank>> PostRank(Rank rank)
        {
            _context.Ranks.Add(rank);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRank", new { id = rank.Id }, rank);
        }

        // DELETE: api/Ranks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Rank>> DeleteRank(Guid id)
        {
            var rank = await _context.Ranks.FindAsync(id);
            if (rank == null)
            {
                return NotFound();
            }

            _context.Ranks.Remove(rank);
            await _context.SaveChangesAsync();

            return rank;
        }

        private bool RankExists(Guid id)
        {
            return _context.Ranks.Any(e => e.Id == id);
        }
    }
}
