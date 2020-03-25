using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FollowersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Followers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Follower>>> GetWatchers()
        {
            return await _context.Watchers.ToListAsync();
        }

        // GET: api/Followers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Follower>> GetFollower(Guid id)
        {
            var follower = await _context.Watchers.FindAsync(id);

            if (follower == null)
            {
                return NotFound();
            }

            return follower;
        }

        // PUT: api/Followers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFollower(Guid id, Follower follower)
        {
            if (id != follower.Id)
            {
                return BadRequest();
            }

            _context.Entry(follower).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FollowerExists(id))
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

        // POST: api/Followers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Follower>> PostFollower(Follower follower)
        {
            _context.Watchers.Add(follower);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFollower", new { id = follower.Id }, follower);
        }

        // DELETE: api/Followers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Follower>> DeleteFollower(Guid id)
        {
            var follower = await _context.Watchers.FindAsync(id);
            if (follower == null)
            {
                return NotFound();
            }

            _context.Watchers.Remove(follower);
            await _context.SaveChangesAsync();

            return follower;
        }

        private bool FollowerExists(Guid id)
        {
            return _context.Watchers.Any(e => e.Id == id);
        }
    }
}
