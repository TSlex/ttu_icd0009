using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApp.Areas.Admin.ApiControllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Area("Admin")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin/{controller}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ProfilesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProfilesController> _logger;
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        public ProfilesController(ILogger<ProfilesController> logger, IConfiguration configuration, SignInManager<Profile> signInManager, UserManager<Profile> userManager)
        {
            _logger = logger;
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] LoginDTO model)
        {
            var profile = await _userManager.FindByEmailAsync(model.Email);
            
            if (profile == null)
            {
                _logger.LogInformation($"Web-Api login. Profile with credentials: {model.Email} - was not found!");
                return StatusCode(403);
            }
            
            var result = await _signInManager.CheckPasswordSignInAsync(profile, model.Password, false);
            
            if (result.Succeeded)
            {
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(profile); //get the User analog
                
                var jwt = IdentityExtensions.GenerateJWT(claimsPrincipal.Claims,
                    _configuration["JWT:SigningKey"],
                    _configuration["JWT:Issuer"],
                    _configuration.GetValue<int>("JWT:ExpirationInDays")
                );
                
                _logger.LogInformation($"Token generated for user {model.Email}");
                return Ok(new {token = jwt, status = "Logged in"});
            }
            
            _logger.LogInformation($"Web-Api login. Profile with credentials: {model.Email} - was attempted to log-in with bad password!");
            return StatusCode(403);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<string>> Register([FromBody] RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = new Profile() { UserName = model.Email, Email = model.Email, EmailConfirmed = true};
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    
                    return Ok(new {status = "Registration was successful!"});
                }
                
                var errors = result.Errors.Select(error => error.Description).ToList();

                return ValidationProblem(JsonSerializer.Serialize(errors));
            }
            
            return StatusCode(403);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public class LoginDTO
        {
            /// <summary>
            /// 
            /// </summary>
            public string Email { get; set; } = default!;
            /// <summary>
            /// 
            /// </summary>
            public string Password { get; set; } = default!;
        }

        /// <summary>
        /// 
        /// </summary>
        public class RegisterDTO
        {
            /// <summary>
            /// 
            /// </summary>
            public string Email { get; set; } = default!;
            /// <summary>
            /// 
            /// </summary>
            public string Password { get; set; } = default!;
        }

        
//        private readonly ApplicationDbContext _context;
//
//        public ProfilesController(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//
//        // GET: api/Profiles
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Profile>>> GetProfiles()
//        {
//            return await _context.Profiles.ToListAsync();
//        }
//
//        // GET: api/Profiles/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Profile>> GetProfile(string id)
//        {
//            var profile = await _context.Profiles.FindAsync(id);
//
//            if (profile == null)
//            {
//                return NotFound();
//            }
//
//            return profile;
//        }
//
//        // PUT: api/Profiles/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
//        // more details see https://aka.ms/RazorPagesCRUD.
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutProfile(Guid id, Profile profile)
//        {
//            if (id != profile.Id)
//            {
//                return BadRequest();
//            }
//
//            _context.Entry(profile).State = EntityState.Modified;
//
//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!ProfileExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//
//            return NoContent();
//        }
//
//        // POST: api/Profiles
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
//        // more details see https://aka.ms/RazorPagesCRUD.
//        [HttpPost]
//        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
//        {
//            _context.Profiles.Add(profile);
//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateException)
//            {
//                if (ProfileExists(profile.Id))
//                {
//                    return Conflict();
//                }
//                else
//                {
//                    throw;
//                }
//            }
//
//            return CreatedAtAction("GetProfile", new { id = profile.Id }, profile);
//        }
//
//        // DELETE: api/Profiles/5
//        [HttpDelete("{id}")]
//        public async Task<ActionResult<Profile>> DeleteProfile(string id)
//        {
//            var profile = await _context.Profiles.FindAsync(id);
//            if (profile == null)
//            {
//                return NotFound();
//            }
//
//            _context.Profiles.Remove(profile);
//            await _context.SaveChangesAsync();
//
//            return profile;
//        }
//
//        private bool ProfileExists(Guid id)
//        {
//            return _context.Profiles.Any(e => e.Id == id);
//        }
    }
}
