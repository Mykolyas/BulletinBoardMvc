using BulletinBoardMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BulletinBoardMvc.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly HttpClient _httpClient;

        public AnnouncementController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(string category, string subcategory)
        {
            var query = $"https://localhost:7204/api/Announcements";

            // додаємо параметри, якщо передані
            if (!string.IsNullOrEmpty(category) || !string.IsNullOrEmpty(subcategory))
            {
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(category))
                    queryParams.Add($"category={Uri.EscapeDataString(category)}");
                if (!string.IsNullOrEmpty(subcategory))
                    queryParams.Add($"subcategory={Uri.EscapeDataString(subcategory)}");

                query += "?" + string.Join("&", queryParams);
            }

            var response = await _httpClient.GetAsync(query);
            if (response.IsSuccessStatusCode)
            {
                var announcements = await response.Content.ReadAsAsync<List<Announcement>>();

                // передаємо у ViewBag список категорій і підкатегорій
                ViewBag.Categories = announcements.Select(a => a.Category).Distinct().OrderBy(c => c).ToList();
                ViewBag.SubCategories = announcements.Select(a => a.SubCategory).Distinct().OrderBy(s => s).ToList();
                ViewBag.SelectedCategory = category;
                ViewBag.SelectedSubCategory = subcategory;

                return View(announcements);
            }

            var errorText = await response.Content.ReadAsStringAsync();
            return Content($"❌ Помилка при отриманні оголошень. Статус: {response.StatusCode}\nПовідомлення: {errorText}");
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Announcement announcement)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7204/api/Announcements", announcement);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var errorText = await response.Content.ReadAsStringAsync();
            return Content($"❌ Помилка при створенні оголошення. Статус: {response.StatusCode}\nПовідомлення: {errorText}");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7204/api/Announcements/{id}");
            if (response.IsSuccessStatusCode)
            {
                var announcement = await response.Content.ReadFromJsonAsync<Announcement>();
                return View(announcement);
            }

            var errorText = await response.Content.ReadAsStringAsync();
            return Content($"❌ Помилка при отриманні оголошення. Статус: {response.StatusCode}\nПовідомлення: {errorText}");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Announcement announcement)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7204/api/Announcements/{announcement.Id}", announcement);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var errorText = await response.Content.ReadAsStringAsync();
            return Content($"❌ Помилка при редагуванні оголошення. Статус: {response.StatusCode}\nПовідомлення: {errorText}");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7204/api/Announcements/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var errorText = await response.Content.ReadAsStringAsync();
            return Content($"❌ Помилка при видаленні оголошення. Статус: {response.StatusCode}\nПовідомлення: {errorText}");
        }
    }
}
