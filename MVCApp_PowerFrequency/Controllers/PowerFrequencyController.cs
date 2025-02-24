using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MVCApp_PowerFrequency.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace MVCApp_PowerFrequency.Controllers
{
    public class PowerFrequencyController : Controller
    {
        private readonly ILogger<PowerFrequencyController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseApiUrl;

        public PowerFrequencyController(ILogger<PowerFrequencyController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _baseApiUrl = configuration["ApiUrl"] ?? throw new InvalidOperationException("API URL is not configured.");
        }

        // GET: PowerFrequency/Index
        public IActionResult Index()
        {
            return View(new PowerFrequencyData());
        }

        // POST: PowerFrequency/LogFrequency
        [HttpPost]
        public async Task<IActionResult> LogFrequency(PowerFrequencyData frequencyData)
        {
            // اعتبارسنجی داده‌ها
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid data. Please check the input values.";
                return View("Index", frequencyData);
            }

            try
            {
                using var httpClient = _httpClientFactory.CreateClient();

                // تبدیل داده‌ها به JSON
                var jsonContent = JsonSerializer.Serialize(frequencyData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // ارسال داده‌ها به API
                var response = await httpClient.PostAsync($"{_baseApiUrl}/Frequency/log-frequency", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Data submitted successfully!";
                    _logger.LogInformation("Frequency data sent successfully.");
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    TempData["Message"] = $"Error submitting data: {response.StatusCode} - {errorResponse}";
                    _logger.LogWarning($"Failed to send frequency data. Status Code: {response.StatusCode}, Response: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending frequency data.");
                TempData["Message"] = "An error occurred while submitting data.";
            }

            return RedirectToAction("Index");
        }

        // صفحه خطا
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
