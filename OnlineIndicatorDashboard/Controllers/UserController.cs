using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineIndicatorDashboard.Models;
using System.Text;

namespace OnlineIndicatorDashboard.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController()
        {

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri("https://localhost:7026/api/"); // Change this to match your API URL
        }

        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetAsync("user");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(content);
            return View(users);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(string name)
        {
            var content = new StringContent(JsonConvert.SerializeObject(name), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("user", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(responseContent);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> GetUsers()
        {
            var response = await _httpClient.GetAsync("user");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(content);
            return Json(users);
        }


        [HttpGet("stream-updates")]
        public async Task CallStreamUpdates()
        
        {
            var response = Response;
            response.Headers.Add("Content-Type", "text/event-stream");

            while (true) // Keep the connection open
            {
                var allUsers = await _httpClient.GetAsync("stream-updates");
                var updatedUsersJson = JsonConvert.SerializeObject(allUsers);

                byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes($"data: {updatedUsersJson}\n\n");
                await response.Body.WriteAsync(dataBytes);

                //response.Body.Flush(); // You may need to use 'response.Body.FlushAsync()'. Check based on your .NET version

                await Task.Delay(5000); // Delay between each update (5 seconds here)
            }
        }
    }
}
