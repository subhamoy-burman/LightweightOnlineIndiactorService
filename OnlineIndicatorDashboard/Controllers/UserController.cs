using Microsoft.AspNetCore.Mvc;
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
    }
}
