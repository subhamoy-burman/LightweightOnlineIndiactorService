using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineIndicator.API.Models;

namespace OnlineIndicator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHeartbeatTime(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.LastHeartbeatTime);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateHeartbeatTime(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.LastHeartbeatTime = DateTime.UtcNow;
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();

            return Ok(user.LastHeartbeatTime);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHeartbeatTimes()
        {
            var users = await _dbContext.Users.ToListAsync();
            //var heartbeatTimes = users.Select(u => u.);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> InsertUser([FromBody] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var user = new User
            {
                UserName = name,
                LastHeartbeatTime = DateTime.UtcNow
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok(user);
        }

    }
}
