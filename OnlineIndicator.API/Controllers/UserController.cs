using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineIndicator.API.Helper;
using OnlineIndicator.API.Models;
using System.Net.Http;
using System.Text;

namespace OnlineIndicator.API.Controllers
{
    public class HeartbeatEvent
    {
        public DateTime HeartbeatTime { get; set; }

        public HeartbeatEvent(DateTime heartbeatTime)
        {
            HeartbeatTime = heartbeatTime;
        }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IEventIdProvider _eventIdProvider;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserController(AppDbContext dbContext, IEventIdProvider eventIdProvider, IHttpContextAccessor contextAccessor)
        {
            _eventIdProvider = eventIdProvider;
            _contextAccessor = contextAccessor;
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

            //var sse = new ServerSentEvent
            //{
            //    Id = _eventIdProvider.GetNextId(),
            //    Type = "heartbeat",
            //    Data = user.LastHeartbeatTime.ToString("o") // Use ISO 8601 formatting for the date
            //};
            //await _contextAccessor.HttpContext.Response.WriteSseEventAsync(sse);


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

        

        [HttpGet("stream-updates")] //Change to unique path

        public async Task StreamUpdates()
        {
            var response = Response;
            response.Headers.Add("Content-Type", "text/event-stream");

            while (true) // keep the stream open
            {
                var users = _dbContext.Users.ToList();
                var updatedUsersJson = JsonConvert.SerializeObject(users);

                // Convert string data to bytes
                byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes($"data: {updatedUsersJson}\n\n");

                // Write asynchronously to the response body
                await response.Body.WriteAsync(new ReadOnlyMemory<byte>(dataBytes));


                //  response.Body.Flush();

                await Task.Delay(5000); // check for updates every 5 seconds.
            }
        }

    }
}
