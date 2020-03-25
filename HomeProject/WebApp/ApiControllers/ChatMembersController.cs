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
    public class ChatMembersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ChatMembers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatMember>>> GetChatMembers()
        {
            return await _context.ChatMembers.ToListAsync();
        }

        // GET: api/ChatMembers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatMember>> GetChatMember(Guid id)
        {
            var chatMember = await _context.ChatMembers.FindAsync(id);

            if (chatMember == null)
            {
                return NotFound();
            }

            return chatMember;
        }

        // PUT: api/ChatMembers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatMember(Guid id, ChatMember chatMember)
        {
            if (id != chatMember.Id)
            {
                return BadRequest();
            }

            _context.Entry(chatMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatMemberExists(id))
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

        // POST: api/ChatMembers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ChatMember>> PostChatMember(ChatMember chatMember)
        {
            _context.ChatMembers.Add(chatMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatMember", new { id = chatMember.Id }, chatMember);
        }

        // DELETE: api/ChatMembers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ChatMember>> DeleteChatMember(Guid id)
        {
            var chatMember = await _context.ChatMembers.FindAsync(id);
            if (chatMember == null)
            {
                return NotFound();
            }

            _context.ChatMembers.Remove(chatMember);
            await _context.SaveChangesAsync();

            return chatMember;
        }

        private bool ChatMemberExists(Guid id)
        {
            return _context.ChatMembers.Any(e => e.Id == id);
        }
    }
}
