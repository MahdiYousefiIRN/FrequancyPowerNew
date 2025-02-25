using Microsoft.AspNetCore.Mvc;
using MVCApp_PowerFrequency.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace MVCApp_PowerFrequency.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseApiUrl;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _baseApiUrl = configuration["ApiUrl"] ?? throw new InvalidOperationException("API URL is not configured.");
        }

        public IActionResult Index()
        {
            return View(new PowerFrequencyData());
        }
        public IActionResult LiveFrequency()
        {
            return View(); // مطمئن شوید که فایل `LiveFrequency.cshtml` در مسیر صحیح وجود دارد
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogFrequency(PowerFrequencyData frequencyData)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning($"Model validation failed: {errors}");
                TempData["Message"] = "Invalid data.";
                return View("Index", frequencyData);
            }

            try
            {
                // ترکیب تاریخ و زمان به DateTime
                if (!string.IsNullOrEmpty(frequencyData.Date) && !string.IsNullOrEmpty(frequencyData.Time))
                {
                    var dateTimeString = $"{frequencyData.Date} {frequencyData.Time}";
                    frequencyData.Timestamp = DateTime.Parse(dateTimeString);
                }

                _logger.LogInformation($"Sending frequency data: {JsonSerializer.Serialize(frequencyData)}");

                var response = await SendDataToApiAsync(frequencyData);

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


        private async Task<HttpResponseMessage> SendDataToApiAsync(PowerFrequencyData frequencyData)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            var jsonContent = JsonSerializer.Serialize(frequencyData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            return await httpClient.PostAsync($"{_baseApiUrl}/Frequency/log-frequency", content);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
