using Microsoft.AspNetCore.Mvc;
using profile;
using Profile.Shared;
using System.Net.Http.Json;

namespace Profile.MVC.Controllers
{
    public class ProfileMVCController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProfileMVCController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();

            _httpClient.BaseAddress =
                new Uri("https://localhost:7296/api/ProfileAPI/");
        }

        [HttpGet("")]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var response = await _httpClient.PostAsJsonAsync(
                "Login",
                login
            );

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Username ya password galat hai."
                );

                return View(login);
            }

            Response.Cookies.Append("UserName", login.username, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(20)
            });
            Response.Cookies.Append("Role", login.role);
            HttpContext.Session.SetString("UserName", login.username);
            HttpContext.Session.SetString("Role", login.role);
            // API se successful login ke baad
            return RedirectToAction(nameof(Index));
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("GetAllProfiles");

            if (!response.IsSuccessStatusCode)
                return View(new List<Profilemodel>());

            var profiles =
                await response.Content.ReadFromJsonAsync<List<Profilemodel>>();
            ViewBag.Username = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("Role");
            return View(profiles);
        }

        // GET: /ProfileMVC/Create
        [HttpGet("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /ProfileMVC/Create
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(Register profile)
        {
            if (!ModelState.IsValid)
                return View(profile);

            var response = await _httpClient.PostAsJsonAsync(
                "CreateProfile",
                profile
            );

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Profile create nahi hua."
                );

                return View(profile);

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /ProfileMVC/Edit/{id}
        [HttpGet("[action]")]
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _httpClient.GetAsync(
                $"GetProfileById/{id}"
            );

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var profile =
                await response.Content.ReadFromJsonAsync<Profilemodel>();

            if (profile == null)
                return NotFound();

            return View(profile);
        }

        // POST: /ProfileMVC/Edit
        [HttpPost("[action]")]
        public async Task<IActionResult> Edit(Profilemodel profile)
        {
            if (!ModelState.IsValid)
                return View(profile);

            var response = await _httpClient.PutAsJsonAsync(
                "UpdateProfile",
                profile
            );

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Profile update nahi hua."
                );

                return View(profile);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /ProfileMVC/Details/{id}
        [HttpGet("[action]")]
        public async Task<IActionResult> Details(string id)
        {
            var response = await _httpClient.GetAsync(
                $"GetProfileById/{id}"
            );

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var profile =
                await response.Content.ReadFromJsonAsync<Profilemodel>();

            if (profile == null)
                return NotFound();

            return View(profile);
        }

        // GET: /ProfileMVC/Delete/{id}
        [HttpGet("[action]")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _httpClient.GetAsync(
                $"GetProfileById/{id}"
            );

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var profile =
                await response.Content.ReadFromJsonAsync<Profilemodel>();

            if (profile == null)
                return NotFound();

            return View(profile);
        }

        // POST: /ProfileMVC/Delete/{id}
        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var response = await _httpClient.DeleteAsync(
                $"DeleteProfile/{id}"
            );

            if (!response.IsSuccessStatusCode)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}